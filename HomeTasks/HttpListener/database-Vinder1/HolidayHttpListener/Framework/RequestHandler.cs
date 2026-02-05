using System.Net;
using HolidayHttpListener.Framework.Controller.Tree;
using HolidayHttpListener.Framework.Exceptions;
using HolidayHttpListener.Repositories;

namespace HolidayHttpListener.Framework;

public sealed class RequestHandler(string url)
{
    public string Url => url;
    public ControllerTree ControllerTree { get; } = new();
    public LocalFileProviderBuilder LocalFileProviderBuilder { get; } = new();
    private LocalFileProvider? LocalFileProvider { get; set; }
    private DatabaseConnectionProvider DBConnectionProvider { get; init; }
        = new DatabaseConnectionProvider("db_connect.json");

    public IExceptionHandler? ExceptionHandler { get; set; }

    private CancellationTokenSource cancellationTokenSource = new();

    public void Build()
    {
        ControllerTree.Build();
        LocalFileProvider = LocalFileProviderBuilder.Build();
    }

    public void Invoke(HttpListenerContext context)
    {
        var response = context.Response;
        var request = context.Request;
        var path = request.Url?.LocalPath;

        _ = Task.Run(async () =>
        {
            await Respond(path, context);
            response.OutputStream.Close();
            response.Close();
        });
    }

    private async Task Respond(string? path, HttpListenerContext context)
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
            DBConnectionProvider = DBConnectionProvider,
            CancellationToken = cancellationTokenSource.Token
        };

        try
        {
            await ControllerTree.Invoke(appContext, path![1..]);
            return;
        }
        catch (Exception ex)
        {
            await ExceptionHandler?.OnException(appContext, ex)!;
        }
    }
}
