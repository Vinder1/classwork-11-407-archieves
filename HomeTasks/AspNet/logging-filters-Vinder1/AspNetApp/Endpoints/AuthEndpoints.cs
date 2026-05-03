using AspNetApp.Dto;
using AspNetApp.IServices;
using AspNetApp.Services;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public class AuthEndpoints //: ICarterModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth").WithTags("Auth");

        group.MapPost("/sign-in", async (
            [FromServices] IAuthService authService,
            [FromServices] IPlayerService playerService,
            HttpContext httpContext,
            IValidator<SignInRequest> validator,
            [FromServices] ILogger<AuthService> logger,
            SignInRequest request) =>
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                if (await playerService.PlayerLoginExistsAsync(request.Login))
                {
                    return Results.BadRequest(new {
                        error="Пользователь с таким логином уже существует"
                    });
                }

                logger.LogInformation("Attempt to sign up from {@Request}", request);

                var user = await authService.SignInNewPlayerAsync(
                    name: request.Name, login: request.Login, password: request.Password);

                var token = authService.GetJwtTokenForUser(user);

                await authService.SaveUserToCookie(user, httpContext);

                return Results.Ok(new
                {
                    token
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        }).WithSummary("Регистрация");

        group.MapPost("/log-in", async (
            [FromServices] IAuthService authService,
            HttpContext httpContext,
            IValidator<LogInRequest> validator,
            [FromServices] ILogger<AuthService> logger,
            LogInRequest request) =>
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                logger.LogInformation("Attempt to log in from {@Request}", request);
                
                var user = await authService.LogInAsync(
                    login: request.Login, 
                    password: request.Password);

                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var token = authService.GetJwtTokenForUser(user);

                await authService.SaveUserToCookie(user, httpContext);

                return Results.Ok(new
                {
                    token
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        }).WithSummary("Вход");
    }
}