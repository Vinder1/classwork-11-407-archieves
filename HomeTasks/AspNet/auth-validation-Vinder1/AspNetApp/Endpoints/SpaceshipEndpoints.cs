using AspNetApp.Configuration;
using AspNetApp.Dto;
using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public static class SpaceshipEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public static void MapSpaceshipEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("spaceship").WithTags("Spaceship");

        group.MapPost("/create",
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

        group.MapGet("/find",
            async (ISpaceshipService spaceshipService, [FromQuery] Guid owner) =>
        {
            try
            {
                var spaceship = await spaceshipService.GetSpaceshipAsync(owner);
                return Results.Ok(new
                {
                    spaceship.OwnerId,
                    spaceship.LocatedAngleMilliradians,
                    spaceship.LocatedRadius
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        group.MapGet("/all",
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

        group.MapPost("/move",
            async (ISpaceshipService spaceshipService,
            [FromServices] IMapper mapper,
            [FromBody] MoveSpaceshipRequest request) =>
        {
            try
            {
                var star = mapper.Map<Star>(request);
                var spaceships = await spaceshipService.MoveSpaceshipAsync(request.OwnerId, star);
                return Results.Ok(new
                {
                    spaceships.OwnerId,
                    radius = spaceships.LocatedRadius,
                    angle = spaceships.LocatedAngleMilliradians
                });
            }
            catch (StarNotFoundException e)
            {
                return Results.NotFound(new { error = e.Message });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        group.MapGet("/has-active",
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
        group.MapDelete("/clean",
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
        }).RequireAuthorization(AuthPolicies.AdminOnly);
    }
}