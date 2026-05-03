using AspNetApp.Dto;
using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IServices;
using AutoMapper;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public class StarVisitEndpoints : ICarterModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/star-visit").WithTags("Star Visit");

        group.MapPost("/create", async (
            [FromBody] StarVisitRequest request,
            [FromServices] IMapper mapper,
            IStarVisitService starVisitService) =>
        {
            var ownerId = request.OwnerId;

            if (ownerId == Guid.Empty)
                return Results.BadRequest(new { error = "OwnerId is required" });

            var star = mapper.Map<Star>(request);

            try
            {
                await starVisitService.Create(ownerId, star);
                return Results.Ok(new { message = "Visit created successfully" });
            }
            catch (StarNotFoundException e)
            {
                return Results.NotFound(new { error = e.Message });
            }
            catch
            {
                return Results.BadRequest();
            }
        })
        .WithName("CreateStarVisit")
        .WithDescription("Creates a spaceship visit to a star")
        .Produces(200)
        .Produces(400)
        .Produces(404);

        // Проверяет, есть ли активный визит (не старше 1 дня)
        group.MapGet("/exists-active", async (
            [FromQuery] Guid ownerId,
            [FromQuery] int radius,
            [FromQuery] int angleMilliradians,
            IStarVisitService starVisitService) =>
        {
            if (ownerId == Guid.Empty)
                return Results.BadRequest(new { error = "OwnerId is required" });

            var star = new Star
            {
                Radius = radius,
                AngleMilliradians = angleMilliradians
            };

            try
            {
                var exists = await starVisitService.ExistsActiveFor(ownerId, star);
                return Results.Ok(new { exists });
            }
            catch (StarNotFoundException e)
            {
                return Results.NotFound(new { error = e.Message });
            }
            catch
            {
                return Results.BadRequest();
            }
        })
        .WithName("ExistsActiveVisit")
        .WithDescription("Checks if there is an active visit (not older than 1 day)")
        .Produces(200)
        .Produces(400);

        // Получает активный визит (не старше 1 дня)
        group.MapGet("/active", async (
            [FromQuery] Guid ownerId,
            [FromQuery] int radius,
            [FromQuery] int angleMilliradians,
            IStarVisitService starVisitService) =>
        {
            if (ownerId == Guid.Empty)
                return Results.BadRequest(new { error = "OwnerId is required" });

            var star = new Star
            {
                Radius = radius,
                AngleMilliradians = angleMilliradians
            };

            try
            {
                var visit = await starVisitService.GetActiveFor(ownerId, star);

                if (visit == null)
                    return Results.NotFound(new { message = "No active visit found" });

                return Results.Ok(new
                {
                    visit.Value.Spaceship.OwnerId,
                    visit.Value.Star.Radius,
                    visit.Value.Star.AngleMilliradians,
                    visit.Value.VisitedAt
                });
            }
            catch (StarNotFoundException e)
            {
                return Results.NotFound(new { error = e.Message });
            }
            catch
            {
                return Results.BadRequest();
            }
        })
        .WithName("GetActiveVisit")
        .WithDescription("Gets the active visit (not older than 1 day)")
        .Produces(200)
        .Produces(400)
        .Produces(404);
    }
}