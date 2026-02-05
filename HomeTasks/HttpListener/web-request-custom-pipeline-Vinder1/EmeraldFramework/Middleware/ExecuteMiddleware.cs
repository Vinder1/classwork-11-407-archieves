namespace Emerald.Middleware;

public class ExecuteMiddleware : IMiddleware
{
    public IMiddleware? Next { get; set; }

    public async Task InvokeAsync(AppContext context)
    {
        await context.EndPoint.InvokeAsync();
    }
}
