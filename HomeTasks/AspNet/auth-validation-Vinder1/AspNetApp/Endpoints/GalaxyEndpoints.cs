using AspNetApp.Configuration;
using AspNetApp.IServices;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public static class GalaxyEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public static void MapGalaxyEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("galaxy").WithTags("Galaxy");

        // Only admins must have access to this endpoint
        group.MapPost("/regenerate", async (IGalaxyService galaxyService, ISpaceshipService spaceshipService) =>
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
        }).RequireAuthorization(AuthPolicies.AdminOnly);
        
        group.MapGet("/", async (IGalaxyService galaxyService) =>
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