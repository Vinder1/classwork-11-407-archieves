using System.Net;

namespace HolidayHttpListener.Framework;

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

    public async Task RunAsync(CancellationToken token = default)
    {
        listener.Start();
        handler.Build();

        while (listener.IsListening)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            handler.Invoke(await listener.GetContextAsync());
        }

        listener.Stop();
    }
}
