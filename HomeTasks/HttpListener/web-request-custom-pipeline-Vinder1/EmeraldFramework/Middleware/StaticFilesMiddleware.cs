namespace Emerald.Middleware;

public class StaticFilesMiddleware(StaticFileProvider fileProvider) : IMiddleware
{
    public IMiddleware? Next { set; get; }

    public async Task InvokeAsync(Emerald.AppContext context)
    {
        var path = context.Request.Url?.LocalPath;
        if (path != null && path.Contains('.') && fileProvider.FileExists(path))
        {
            if (Path.GetExtension(path) == ".html")
            {
                context.Response.StatusCode = 403;
                return;
            }

            await fileProvider.SendFile(context, path);
            return;
        }

        if (Next != null)
            await Next.InvokeAsync(context);
    }
}
