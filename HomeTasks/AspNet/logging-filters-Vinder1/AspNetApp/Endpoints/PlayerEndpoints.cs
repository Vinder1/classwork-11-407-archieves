using System.Security.Claims;
using AspNetApp.Configuration;
using AspNetApp.Dto;
using AspNetApp.IServices;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public class PlayerEndpoints : ICarterModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("player").WithTags("Player");

        app.MapGet("/me", (ClaimsPrincipal user) =>
        {
            return Results.Ok(new
            {
                Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Name = user.FindFirst("playerName")?.Value,
                Login = user.FindFirst(ClaimTypes.Name)?.Value,
                Role = user.FindFirst(ClaimTypes.Role)?.Value,
                AuthType = user.Identity?.AuthenticationType // "JwtBearer" или "Cookies"
            });
        }).RequireAuthorization(AuthPolicies.Any);

        group.MapGet("/", async ([FromServices] IPlayerService playerService, Guid id) =>
        {
            try
            {
                var player = await playerService.GetPlayerAsync(id);
                return Results.Ok(new
                {
                    player.Id,
                    player.Name
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        group.MapPost("/new-name", async (
            [FromServices] IPlayerService playerService,
            IValidator<UpdatePlayerNameRequest> validator,
            UpdatePlayerNameRequest request) =>
        {
            try
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var player = await playerService.UpdatePlayerNameAsync(request.Id, request.Name);
                return Results.Ok(new
                {
                    player.Id,
                    newName = player.Name
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        }).RequireAuthorization(AuthPolicies.Any);

        group.MapDelete("/", async ([FromServices] IPlayerService playerService, Guid id) =>
        {
            try
            {
                await playerService.DeletePlayerAsync(id);
                return Results.Ok();
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        group.MapGet("/exists", async ([FromServices] IPlayerService playerService, Guid id) =>
        {
            try
            {
                return Results.Ok(new
                {
                    exists = await playerService.PlayerExistsAsync(id)
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
    }
}