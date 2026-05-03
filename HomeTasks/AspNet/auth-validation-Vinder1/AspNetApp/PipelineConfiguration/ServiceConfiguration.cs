using AspNetApp.IServices;
using AspNetApp.Services;

namespace AspNetApp.PipelineConfiguration;

/// <summary>
/// 
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPlayerService, PlayerService>();
        builder.Services.AddScoped<ISpaceshipService, SpaceshipService>();
        builder.Services.AddScoped<IGalaxyService, GalaxyService>();
        builder.Services.AddScoped<IStarVisitService, StarVisitService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<ICookieService, CookieService>();
    }
}