using AspNetApp.Mapping;

namespace AspNetApp.PipelineConfiguration;

/// <summary>
/// 
/// </summary>
public static class AutoMapperConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureAutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AppToDbMappingProfile>();
            cfg.AddProfile<AppToDbEndpointsProfile>();
            cfg.AllowNullCollections = false;
        });

        builder.Services.AddSingleton<GalaxyConverter>();
        builder.Services.AddSingleton<GalaxyReverseConverter>();
    }
}