using HolidayApp.Dto;
using Emerald;
using Emerald.Controller;
using HolidayApp.Repositories;
using HolidayApp.Validation;
using Emerald.Exceptions;
using HolidayApp.Services;

namespace HolidayApp.Controllers;

[Controller]
public class UserController(UserRepository userRepository, JwtAuth jwtAuth)
{
    private UserRepository Repository => userRepository;
    private JwtAuth JwtAuth => jwtAuth;

    [Controller.Get("signup")]
    public async Task SendSignUpPage(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("signup.html");
    }

    [Controller.Post("signup")]
    public async Task SignUpPost(Emerald.AppContext context)
    {
        var signupInfo = await context.ReadJson<SignUpDto>(context.CancellationToken);

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
            await context.WriteJson(new
            {
                errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            });
            return;
        }

        var id = await Repository.CreateUserAsync(
            signupInfo,
        context.CancellationToken);

        if (id == -1)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var userRow = await Repository.GetByIdAsync(
            id: id.Value,
        context.CancellationToken);

        var refreshToken = context.Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            var res2 = JwtAuth.GenerateRefreshToken(userRow!.Id);
            context.Response.Cookies.Add(new System.Net.Cookie("refreshToken", res2)
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14)
            });
        }

        var token = JwtAuth.GenerateJwtToken(userRow!.Id);

        context.SetResponseStatusCode(StatusCode.Created201);
        await context.WriteJson(new { token });
    }

    [Controller.Get("signin")]
    public async Task SendSignInPage(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("signin.html");
    }

    [Controller.Post("signin")]
    public async Task TrySignIn(Emerald.AppContext context)
    {
        var loginDto = await context.ReadJson<LoginDto>(context.CancellationToken);

        if (loginDto == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var userRow = await Repository.GetByLoginAsync(
            login: loginDto.Login,
        context.CancellationToken);

        if (userRow == null || !PasswordHasher.Validate($"{userRow.Hash};{userRow.Salt}", loginDto.Password))
        {
            context.SetResponseStatusCode(StatusCode.Unauthorized401);
            return;
        }

        var refreshToken = context.Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            var res2 = JwtAuth.GenerateRefreshToken(userRow.Id);
            context.Response.Cookies.Add(new System.Net.Cookie("refreshToken", res2)
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14)
            });
        }

        var token = JwtAuth.GenerateJwtToken(userRow.Id);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { token });
    }

    [Controller.Get("profile")]
    public async Task GetProfile(Emerald.AppContext context)
    {
        context.SetResponseStatusCode(StatusCode.OK);
        await context.SendFile("profile.html");
    }

    [Controller.Put("profile/update")]
    public async Task UpdateInfo(Emerald.AppContext context)
    {
        var (isSuccess, id) = JwtAuth.ValidateJwtToken(context.GetTokenFromHeader());
        if (!isSuccess || id == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var updateInfo = await context.ReadJson<UpdateUserDto>(context.CancellationToken);
        if (updateInfo == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var validator = new UserUpdateValidator();
        var result = await validator.ValidateAsync(updateInfo, context.CancellationToken);

        if (!result.IsValid)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            await context.WriteJson(new
            {
                errors = result.Errors.Select(e => e.ErrorMessage).ToList()
            });
            return;
        }

        var success = await Repository.ForceUpdateUserAsync(
            id: id.Value,
            info: updateInfo,
        context.CancellationToken);

        if (success)
        {
            context.SetResponseStatusCode(StatusCode.OK);
            await context.WriteJson(new { redirect = "/profile" });
        }
        else
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }
    }

    [Controller.Delete("profile")]
    public async Task DeleteUser(Emerald.AppContext context)
    {
        var (isSuccess, id) = JwtAuth.ValidateJwtToken(context.GetTokenFromHeader());
        if (!isSuccess || id == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        await Repository.DeleteUserAsync(
            id: id.Value,
        context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { redirect = "/home" });
    }
}
