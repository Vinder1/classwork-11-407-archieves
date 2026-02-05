using System.Net;
using Emerald.Exceptions;
using Emerald.Middleware;

namespace Emerald;

public sealed class RequestHandler
{
    public LocalFileProvider? LocalFileProvider { get; init; }
    public required MiddlewarePipeline Pipeline { get; init; }
    public IExceptionHandler? ExceptionHandler { get; set; }

    private readonly SemaphoreSlim _semaphore = new (50);

    public void Invoke(HttpListenerContext context, CancellationToken appToken)
    {
        var response = context.Response;
        var request = context.Request;
        var path = request.Url?.LocalPath;

        _ = Task.Run(async () =>
        {
            await Respond(path, context, appToken);
            response.OutputStream.Close();
            response.Close();
        }, appToken);
    }

    private async Task Respond(string? path, HttpListenerContext context, CancellationToken appToken)
    {
        if (path == null)
            return;

        if (LocalFileProvider != null && path.Contains('.') && LocalFileProvider.FileExists(path))
        {
            await LocalFileProvider.SendFile(context.Response, path);
            return;
        }

        var appContext = new AppContext
        {
            ListenerContext = context,
            FileProvider = LocalFileProvider,
            CancellationToken = appToken
        };

        await _semaphore.WaitAsync(appToken);
        try
        {
            await Pipeline.InvokeAsync(appContext);
            return;
        }
        catch (Exception ex)
        {
            await ExceptionHandler?.OnException(appContext, ex)!;
        } 
        finally
        {
            _semaphore.Release();
        }
    }
}