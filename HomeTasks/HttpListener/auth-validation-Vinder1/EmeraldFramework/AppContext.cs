using System.Net;

namespace Emerald;

public class AppContext
{
    public required HttpListenerContext ListenerContext { get; init; }
    public HttpListenerRequest Request => ListenerContext.Request;
    public HttpListenerResponse Response => ListenerContext.Response;

    public required LocalFileProvider FileProvider { get; init; }
    public CancellationToken CancellationToken { get; init; }
}
