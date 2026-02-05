using System.Net;
using Emerald.Controller.Tree;
using Emerald.Exceptions;
using Emerald.Services;

namespace Emerald;

public sealed class RequestHandler(string url)
{
    public string Url => url;
    public ControllerTree Controllers { get; } = new();
    public ServiceCollection Services { get; } = new();
    public LocalFileProvider? LocalFileProvider { get; set; }

    public IExceptionHandler? ExceptionHandler { get; set; }

    private readonly SemaphoreSlim _semaphore = new (50);

    public void Build()
    {
        if (LocalFileProvider != null)
            Services.AddInstance(LocalFileProvider);
        Controllers.Build(Services);
    }

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
        var response = context.Response;

        if (path.Contains('.') && LocalFileProvider!.FileExists(path))
        {
            await LocalFileProvider.SendFile(context.Response, path);
            return;
        }

        var appContext = new AppContext
        {
            ListenerContext = context,
            FileProvider = LocalFileProvider!,
            CancellationToken = appToken
        };

        await _semaphore.WaitAsync(appToken);
        try
        {
            await Controllers.Invoke(appContext, path![1..]);
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
