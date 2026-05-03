using AspNetApp.IServices;

namespace AspNetApp;

public static class StarsEndpoints
{
    public static void MapStarsEndpoints(this WebApplication app)
    {
        app.MapGet("/stars/all", async (IGalaxyService galaxyService) =>
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
        });

        // GET /stars/find?radius=5&angle=1.57
        app.MapGet("/stars/find", async (int radius, double angle, IGalaxyService galaxyService) =>
        {
            try
            {
                if (radius < 0)
                {
                    return Results.BadRequest("Radius must be positive");
                }

                if (angle < 0 || angle > 2 * Math.PI)
                {
                    return Results.BadRequest("Angle must be between 0 and 2π");
                }

                var star = await galaxyService.FindStarAsync(radius, angle);

                if (star == null)
                {
                    return Results.NotFound($"Star (radius {radius}, angle {angle}) not found");
                }

                return Results.Ok(star);
            }
            catch
            {
                return Results.InternalServerError();
            }
        });

        // GET /stars/nearby?radius=5&angle=1.57&range=2
        app.MapGet("/stars/nearby", async (int radius, double angle, int range, IGalaxyService galaxyService) =>
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

                var centerStar = await galaxyService.FindStarAsync(radius, angle);

                if (centerStar == null)
                {
                    return Results.NotFound($"Star (radius {radius}, angle {angle}) not found");
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