using System.Net;

namespace HolidayHttpListener;

class Program
{
    private const string Url = "http://localhost:5000/";

    public static async Task Main(string[] args)
    {
        HttpListener listener = new();
        RequestHandler handler = new (Url);
        listener.Prefixes.Add(Url);
        listener.Start();

        while (listener.IsListening)
        {
            handler.Invoke(await listener.GetContextAsync());
        }
    }
}
