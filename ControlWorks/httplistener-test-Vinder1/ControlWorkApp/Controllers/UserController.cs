using ControlWorkApp.Dto;
using Emerald;
using Emerald.Controller;
using ControlWorkApp.Repositories;
using ControlWorkApp.Validation;
using Emerald.Exceptions;
using ControlWorkApp.Services;

namespace ControlWorkApp.Controllers;

[Controller]
public class UserController(UserRepository userRepository)
{
    private UserRepository Repository => userRepository;

    [Controller.Post("user")]
    public async Task SignUpPost(Emerald.AppContext context)
    {
        var signupInfo = await context.ReadJsonAsync<SignUpDto>(context.CancellationToken);

        if (signupInfo == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var validator = new UserCreateValidator();
        var result = await validator.ValidateAsync(signupInfo, context.CancellationToken);

        if (!result.IsValid)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            await context.WriteJsonAsync(new
            {
                errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            });
            return;
        }

        var session = await Repository.CreateUserAsync(
            signupInfo,
        context.CancellationToken);

        if (session == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.Response.Cookies.Add(new System.Net.Cookie("session", session)
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(14)
        });

        context.SetResponseStatusCode(StatusCode.Created201);
    }

    [Controller.Post("auth/login")]
    public async Task TrySignIn(Emerald.AppContext context)
    {
        var loginDto = await context.ReadJsonAsync<LoginDto>(context.CancellationToken);

        if (loginDto == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var userRow = await Repository.GetByLoginAsync(
            login: loginDto.Login,
        context.CancellationToken);

        if (userRow == null || !PasswordHasher.Validate(userRow.Hash, loginDto.Password))
        {
            context.SetResponseStatusCode(StatusCode.Unauthorized401);
            return;
        }

        context.Response.Cookies.Add(new System.Net.Cookie("session", userRow.SessionId)
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(14)
        });

        context.SetResponseStatusCode(StatusCode.OK);
    }
}
