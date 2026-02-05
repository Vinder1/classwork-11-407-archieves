
using Emerald.Controller.Tree;

namespace Emerald.Middleware;

public class EndPoint(ControllerTree tree) : IMiddleware
{
    public IMiddleware? Next { set { } }

    public async Task InvokeAsync(AppContext context)
    {
        var path = context.Request.Url?.LocalPath;
        await tree.Invoke(context, path![1..]);
    }
}
