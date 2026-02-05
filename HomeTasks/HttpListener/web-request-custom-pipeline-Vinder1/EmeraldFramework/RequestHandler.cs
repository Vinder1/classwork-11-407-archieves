using System.Net;
using Emerald.Exceptions;
using Emerald.Middleware;
using Emerald.Services.Logging;

namespace Emerald;

public sealed class RequestHandler
{
    public StaticFileProvider? LocalFileProvider { get; init; }
    public required MiddlewarePipeline Pipeline { get; init; }
    public IExceptionHandler? ExceptionHandler { get; set; }
    public ILogger? Logger { get; set; } 

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

        var appContext = new AppContext
        {
            ListenerContext = context,
            FileProvider = LocalFileProvider,
            CancellationToken = appToken
        };
        
        Logger?.StartRequestProcessing(appContext);

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