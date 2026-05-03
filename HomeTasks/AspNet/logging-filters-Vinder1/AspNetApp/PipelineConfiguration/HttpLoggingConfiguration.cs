using Microsoft.AspNetCore.HttpLogging;

namespace AspNetApp.PipelineConfiguration;

/// <summary>
/// 
/// </summary>
public static class HttpLoggingConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureHttpLogging(this WebApplicationBuilder builder)
    {

        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }
}