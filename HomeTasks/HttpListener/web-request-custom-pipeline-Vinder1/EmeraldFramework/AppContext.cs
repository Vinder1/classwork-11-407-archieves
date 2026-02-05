using System.Net;
using EmeraldFramework.Controller.Tree;

namespace Emerald;

public class AppContext
{
    public required HttpListenerContext ListenerContext { get; init; }
    public HttpListenerRequest Request => ListenerContext.Request;
    public HttpListenerResponse Response => ListenerContext.Response;

    public required StaticFileProvider? FileProvider { get; init; }
    public CancellationToken CancellationToken { get; init; }

    public ControllerEndPoint EndPoint { get; set; } = null!;

    public IAuthDto? AuthInfo { get; set; }
    public object? Model;
}
