using System.Net;
using Emerald.Building;

namespace Emerald;

public partial class HttpApp
{
    private readonly HttpListener listener;
    private AppBuilder builder;
    private RequestHandler? handler;

    public HttpApp(string url)
    {
        listener = new();
        listener.Prefixes.Add(url);
        builder = new();
    }

    public void Build()
    {
        handler = builder.Build();
        builder = new();
    }

    public async Task RunAsync(CancellationToken appToken)
    {
        if (handler == null)
        {
            throw new Exception("Handler is missing. Try invoking method 'Build' first.");
        }
        
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
