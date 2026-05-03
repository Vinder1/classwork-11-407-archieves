using AspNetApp.IServices;

namespace AspNetApp.Endpoints;

public static class GalaxyEndpoints
{
    public static void MapGalaxyEndpoints(this WebApplication app)
    {
        // Only admins must have access to this endpoint
        app.MapPost("/galaxy/regenerate", async (IGalaxyService galaxyService, ISpaceshipService spaceshipService) =>
        {
            try
            {
                var galaxy = await galaxyService.RegenerateGalaxyAsync();
                await spaceshipService.CleanupInactiveSpaceshipsAsync(galaxy.Version);
                return Results.Ok();
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
        
        app.MapGet("/galaxy", async (IGalaxyService galaxyService) =>
        {
            try
            {
                var version = await galaxyService.GetCurrentGalaxyVersionAsync();
                return Results.Ok(new
                {
                    version
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
    }
}