using System.ComponentModel.DataAnnotations;
using AspNetApp.Dto;
using AspNetApp.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public static class AuthEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("auth").WithTags("Auth");

        group.MapPost("/sign-in", async (
            [FromServices] IAuthService authService,
            HttpContext httpContext,
            IValidator<SignInRequest> validator,
            SignInRequest request) =>
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var token = await authService.SignInNewPlayerAsync(
                    name: request.Name, login: request.Login, password: request.Password);

                await authService.SaveUserToCookie(httpContext);

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
            LogInRequest request) =>
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                
                var token = await authService.LogInAsync(
                    login: request.Login, 
                    password: request.Password);

                if (token == null)
                {
                    return Results.Unauthorized();
                }

                await authService.SaveUserToCookie(httpContext);

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