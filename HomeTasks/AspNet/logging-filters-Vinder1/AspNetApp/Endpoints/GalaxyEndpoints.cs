using AspNetApp.Configuration;
using AspNetApp.IServices;
using Carter;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public class GalaxyEndpoints : ICarterModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
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