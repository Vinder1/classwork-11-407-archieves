using TaskTracker.Api.Dtos.Boards;
using TaskTracker.Api.Repositories.Interfaces;
using TaskTracker.Api.Services.Interfaces;

namespace TaskTracker.Api.Endpoints;

public static class BoardEndpoints
{
    public static IEndpointRouteBuilder MapBoardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/boards")
            .WithTags("Boards")
            .RequireAuthorization();

        group.MapGet("/", async (IBoardRepository service, CancellationToken cancellationToken) =>
        {
            var items = await service.GetAllAsync(cancellationToken);
            return Results.Ok(items);
        });

        group.MapPost("/", async (CreateBoardRequest request, IBoardService service, CancellationToken cancellationToken) =>
            {
                var created = await service.CreateAsync(request, cancellationToken);
                return Results.Created($"/api/boards/{created.Id}", created);
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Manager"));

        return app;
    }
}