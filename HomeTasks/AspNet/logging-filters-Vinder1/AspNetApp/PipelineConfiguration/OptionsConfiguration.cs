using AspNetApp.Configuration;

namespace AspNetApp.PipelineConfiguration;

/// <summary>
/// 
/// </summary>
public static class OptionsConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    }
}