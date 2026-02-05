using System.Net;
using HolidayHttpListener.Repositories;

namespace HolidayHttpListener.Framework;

public class AppContext
{
    public required HttpListenerContext ListenerContext { get; init; }
    public HttpListenerRequest Request => ListenerContext.Request;
    public HttpListenerResponse Response => ListenerContext.Response;

    public required LocalFileProvider FileProvider { get; init; }
    public required DatabaseConnectionProvider DBConnectionProvider { get; init; }
    public CancellationToken CancellationToken { get; init; }
}
