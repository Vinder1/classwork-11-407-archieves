using AspNetApp.Dto;
using AspNetApp.Entity;
using AspNetApp.IServices;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp;

public static class SpaceshipEndpoints
{
    public static void MapSpaceshipEndpoints(this WebApplication app)
    {
        app.MapPost("/spaceship/create",
            async (ISpaceshipService spaceshipService, IGalaxyService galaxyService, CreateSpaceshipRequest request) =>
        {
            try
            {
                var version = await galaxyService.GetCurrentGalaxyVersionAsync();
                var spaceship = await spaceshipService.CreateSpaceshipAsync(request.OwnerId, version);
                return Results.Ok();
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        app.MapGet("/spaceship/find",
            async (ISpaceshipService spaceshipService, [FromQuery] Guid owner) =>
        {
            try
            {
                var spaceship = await spaceshipService.GetSpaceshipAsync(owner);
                return Results.Ok(new
                {
                    spaceship.OwnerId,
                    spaceship.LocatedAngle,
                    spaceship.LocatedRadius
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        app.MapGet("/spaceship/all",
            async (ISpaceshipService spaceshipService) =>
        {
            try
            {
                var spaceships = await spaceshipService.GetAllSpaceshipsAsync();
                return Results.Ok(spaceships);
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        app.MapPost("/spaceship/move",
            async (ISpaceshipService spaceshipService, [FromBody] MoveSpaceshipRequest request) =>
        {
            try
            {
                var spaceships = await spaceshipService.MoveSpaceshipAsync(request.OwnerId, request.NewRadius, request.NewAngle);
                return Results.Ok(spaceships);
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        app.MapGet("/spaceship/has-active",
            async (ISpaceshipService spaceshipService, IPlayerService playerService, IGalaxyService galaxyService, [FromQuery] Guid owner) =>
        {
            try
            {
                if (!await playerService.PlayerExistsAsync(owner))
                {
                    return Results.NotFound(new
                    {
                        exists = false,
                        comment = "UserNotFound"
                    });
                }

                var version = await galaxyService.GetCurrentGalaxyVersionAsync();
                var exists = await spaceshipService.HasActiveSpaceshipAsync(owner, version);
                if (exists)
                {
                    return Results.Ok(new
                    {
                        exists,
                        comment = "Ok"
                    });
                }
                else
                {
                    return Results.NotFound(new
                    {
                        exists,
                        comment = "ShipNotFound"
                    });
                }
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        // Admin only!
        app.MapDelete("/spaceship/clean",
            async (ISpaceshipService spaceshipService, IGalaxyService galaxyService) =>
        {
            try
            {
                var version = await galaxyService.GetCurrentGalaxyVersionAsync();
                var exists = await spaceshipService.CleanupInactiveSpaceshipsAsync(version);
                return Results.Ok();
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
    }
}