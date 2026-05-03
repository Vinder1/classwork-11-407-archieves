using AspNetApp.DomainUtility;
using AspNetApp.IServices;
using Carter;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
public class StarsEndpoints : ICarterModule
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("stars").WithTags("Stars");

        group.MapGet("/all", async (IGalaxyService galaxyService) =>
        {
            try
            {
                var stars = await galaxyService.GetAllStarsAsync();
                return Results.Ok(stars);
            }
            catch
            {
                return Results.InternalServerError();
            }
        });//.RequireAuthorization(AuthPolicies.Any);

        // GET /stars/find?radius=5&angle=1.57
        group.MapGet("/find", async (int radius, int angleMilliradians, IGalaxyService galaxyService) =>
        {
            try
            {
                if (radius < 0)
                {
                    return Results.BadRequest("Radius must be positive");
                }

                if (angleMilliradians < 0 || angleMilliradians > Angle.TwoPiMilliradians)
                {
                    return Results.BadRequest($"Angle must be between 0 and {Angle.TwoPiMilliradians}");
                }

                var star = await galaxyService.FindStarAsync(radius, angleMilliradians);

                if (star == null)
                {
                    return Results.NotFound($"Star (radius {radius}, angle {angleMilliradians}) not found");
                }

                return Results.Ok(star);
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        // GET /stars/nearby?radius=5&angleMilliradians=1570&range=2
        group.MapGet("/nearby", async (int radius, int angleMilliradians, int range, IGalaxyService galaxyService) =>
        {
            try
            {
                if (radius < 0)
                {
                    return Results.BadRequest("Radius must be positive");
                }

                if (range < 1)
                {
                    return Results.BadRequest("Range must be greater than 0");
                }

                var centerStar = await galaxyService.FindStarAsync(radius, angleMilliradians);

                if (centerStar == null)
                {
                    return Results.NotFound($"Star (radius {radius}, angle {angleMilliradians}) not found");
                }

                var nearbyStars = await galaxyService.GetStarsNearStarAsync(centerStar.Value, range);

                return Results.Ok(new
                {
                    CenterStar = centerStar,
                    Range = range,
                    NearbyStars = nearbyStars,
                    TotalCount = nearbyStars.Count()
                });
            }
            catch
            {
                return Results.InternalServerError();
            }
        });
    }
}