using Destructurama;
using Serilog;
using Serilog.Events;

namespace AspNetApp.PipelineConfiguration;

/// <summary>
/// 
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {
#if DEBUG
        Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine($"[Serilog SelfLog] {msg}"));
#endif

        builder.Host.UseSerilog((context, configuration) =>
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Destructure.UsingAttributes());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public static void UseCustomSerilogAndHttpLogging(this WebApplication app)
    {
        app.UseWhen(
            predicate: ctx => !ctx.Request.Path.StartsWithSegments("/health") &&
                            !ctx.Request.Path.StartsWithSegments("/metrics") &&
                            !ctx.Request.Path.StartsWithSegments("/favicon.ico"),
            configuration: app =>
            {

                app.UseSerilogRequestLogging(opt =>
                {
                    opt.GetLevel = (ctx, _, ex) =>
                        ex != null || ctx.Response.StatusCode >= 500 ? LogEventLevel.Error :
                        LogEventLevel.Information;

                    opt.EnrichDiagnosticContext = (dContext, httpContext) =>
                    {
                        if (httpContext.Items.TryGetValue("CorrelationId", out var id))
                            dContext.Set("CorrelationId", id);
                    };
                });

                app.UseHttpLogging();
            });
    }
}