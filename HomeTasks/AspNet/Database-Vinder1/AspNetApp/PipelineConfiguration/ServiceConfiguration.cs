using AspNetApp.IServices;
using AspNetApp.Services;

namespace AspNetApp.PipelineConfiguration;

public static class ServiceConfiguration
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPlayerService, PlayerService>();
        builder.Services.AddScoped<ISpaceshipService, SpaceshipService>();
        builder.Services.AddScoped<IGalaxyService, GalaxyService>();
        builder.Services.AddScoped<IStarVisitService, StarVisitService>();
    }
}