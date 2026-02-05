using System.Net;

namespace Emerald;

public partial class HttpApp
{
    private readonly string Url;
    private readonly HttpListener listener;
    private readonly RequestHandler handler;

    public HttpApp(string url)
    {
        Url = url;
        listener = new();
        listener.Prefixes.Add(url);
        handler = new(url);
    }

    public void Build()
    {
        handler.Build();
    }

    public async Task RunAsync(CancellationToken appToken)
    {
        listener.Start();

        while (listener.IsListening)
        {
            if (appToken.IsCancellationRequested)
            {
                return;
            }
            handler.Invoke(await listener.GetContextAsync(), appToken);
        }

        listener.Stop();
    }
}
