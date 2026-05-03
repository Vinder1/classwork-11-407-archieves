namespace AspNetApp.PipelineConfiguration;

/// <summary>
/// 
/// </summary>
public class CorrelationIdMiddleware : IMiddleware
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var id = Guid.NewGuid().ToString("N")[..8];
        context.Items["CorrelationId"] = id;
        context.Response.Headers["X-Correlation-Id"] = id;
        return next.Invoke(context);
    }
}

/// <summary>
/// 
/// </summary>
public static class CorrelationIdMiddlewareExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    public static void UseCorrelationIdGenerator(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
    }
}