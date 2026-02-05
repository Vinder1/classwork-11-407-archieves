namespace Emerald.Middleware;

public class MiddlewarePipeline
{
    private IMiddleware? First;
    private IMiddleware? Last;

    public void Add(IMiddleware middleware)
    {
        if (Last == null)
        {
            First = Last = middleware;
            return;
        }

        Last.Next = middleware;
        Last = middleware;
    }

    public async Task InvokeAsync(AppContext context)
    {
        if (First == null)
            return;
        await First.InvokeAsync(context);
    }
}
