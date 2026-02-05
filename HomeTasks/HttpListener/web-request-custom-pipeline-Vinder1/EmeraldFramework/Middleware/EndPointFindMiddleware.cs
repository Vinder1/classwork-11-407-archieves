using Emerald.Controller.Tree;

namespace Emerald.Middleware;

public class EndPointFindMiddleware(ControllerTree tree) : IMiddleware
{
    public IMiddleware? Next { set; get; }

    public async Task InvokeAsync(AppContext context)
    {
        var path = context.Request.Url?.LocalPath;
        context.EndPoint = tree.Find(context, path![1..]);

        if (Next != null)
            await Next.InvokeAsync(context);
    }
}
