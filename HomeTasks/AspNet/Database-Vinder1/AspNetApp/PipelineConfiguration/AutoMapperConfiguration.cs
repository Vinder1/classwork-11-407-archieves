using AspNetApp.Mapping;

namespace AspNetApp.PipelineConfiguration;

public static class AutoMapperConfiguration
{
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