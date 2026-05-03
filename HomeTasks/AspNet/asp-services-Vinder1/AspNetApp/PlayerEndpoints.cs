using AspNetApp.Dto;
using AspNetApp.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp;

public static class PlayerEndpoints
{
    public static void MapPlayerEndpoints(this WebApplication app)
    {
        app.MapPost("/player/new", async ([FromServices] IPlayerService playerService, CreatePlayerRequest request) =>
        {
            try
            {
                var player = await playerService.CreatePlayerAsync(request.Name);
                return Results.Ok(new
                {
                    player.Id
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
        
        app.MapGet("/player", async ([FromServices] IPlayerService playerService, Guid id) =>
        {
            try
            {
                var player = await playerService.GetPlayerAsync(id);
                return Results.Ok(player);
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
        
        app.MapPost("/player/new-name", async ([FromServices] IPlayerService playerService, UpdatePlayerNameRequest request) =>
        {
            try
            {
                var player = await playerService.UpdatePlayerNameAsync(request.Id, request.Name);
                return Results.Ok(player);
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
        
        app.MapDelete("/player", async ([FromServices] IPlayerService playerService, Guid id) =>
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
        
        app.MapGet("/player/exists", async ([FromServices] IPlayerService playerService, Guid id) =>
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