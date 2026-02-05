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

    public bool Contains<T>()
    {
        if (First == null)
        {
            return false;
        }
        var type = typeof(T);
        var m = First;
        if (m!.GetType() == type)
            return true;
        while (m != null && m != Last)
        {
            m = m.Next;
            if (m != null && m.GetType() == type)
                return true;
        }
        return false;
    }

    public async Task InvokeAsync(AppContext context)
    {
        if (First == null)
            return;
        await First.InvokeAsync(context);
    }
}
