using HolidayApp.Dto;
using Emerald;
using Emerald.Controller;
using HolidayApp.Repositories;
using HolidayApp.Validation;
using Emerald.Exceptions;
using HolidayApp.Services;
using System.ComponentModel.DataAnnotations;
using EmeraldFramework.Controller;

namespace HolidayApp.Controllers;

[Controller]
public class UserController(UserRepository userRepository, JwtAuth jwtAuth)
{
    private UserRepository Repository => userRepository;
    private JwtAuth JwtAuth => jwtAuth;

    [Controller.Get("signup")]
    public async Task SendSignUpPage(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "signup");
    }

    [Controller.Post("signup")]
    [RequireModel<SignUpDto>]
    public async Task SignUpPost(Emerald.AppContext context)
    {
        var signupInfo = (context.Model as SignUpDto)!;

        var id = await Repository.CreateUserAsync(
            signupInfo,
        context.CancellationToken);

        if (id == -1)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var userRow = (await Repository.GetByIdAsync(
            id: id.Value,
        context.CancellationToken))!;

        var authDto = new AuthDto
        {
            Id = userRow.Id,
            Role = userRow.Role,
        };

        var refreshToken = context.Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            var res2 = JwtAuth.GenerateRefreshToken(authDto);
            context.Response.Cookies.Add(new System.Net.Cookie("refreshToken", res2)
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14)
            });
        }

        var token = JwtAuth.GenerateJwtToken(authDto);

        context.SetResponseStatusCode(StatusCode.Created201);
        await context.WriteJson(new { token });
    }

    [Controller.Get("signin")]
    public async Task SendSignInPage(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "signin");
    }

    [Controller.Post("signin")]
    [RequireModel<LoginDto>]
    public async Task TrySignIn(Emerald.AppContext context)
    {
        var loginDto = (context.Model as LoginDto)!;

        var userRow = await Repository.GetByLoginAsync(
            login: loginDto.Login,
        context.CancellationToken);

        if (userRow == null || !PasswordHasher.Validate($"{userRow.Hash};{userRow.Salt}", loginDto.Password))
        {
            context.SetResponseStatusCode(StatusCode.Unauthorized401);
            return;
        }

        var authDto = new AuthDto
        {
            Id = userRow.Id,
            Role = userRow.Role,
        };

        var refreshToken = context.Request.Cookies["refreshToken"];
        if (refreshToken == null)
        {
            var res2 = JwtAuth.GenerateRefreshToken(authDto);
            context.Response.Cookies.Add(new System.Net.Cookie("refreshToken", res2)
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14)
            });
        }

        var token = JwtAuth.GenerateJwtToken(authDto);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { token });
    }

    [Controller.Get("profile")]
    [Authorized(redirect: "/signin")]
    public async Task GetProfile(Emerald.AppContext context)
    {
        await View.RenderHtmlFile(context, "profile");
    }

    [Controller.Put("profile/update")]
    [Authorized]
    [RequireModel<UpdateUserDto>]
    public async Task UpdateInfo(Emerald.AppContext context)
    {
        var updateInfo = (context.Model as UpdateUserDto)!;

        var success = await Repository.ForceUpdateUserAsync(
            id: context.AuthInfo!.Id,
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
    [Authorized]
    public async Task DeleteUser(Emerald.AppContext context)
    {
        await Repository.DeleteUserAsync(
            id: context.AuthInfo!.Id,
        context.CancellationToken);

        context.SetResponseStatusCode(StatusCode.OK);
        await context.WriteJson(new { redirect = "/home" });
    }
}
