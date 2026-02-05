using System.Diagnostics;
using Emerald.Middleware;
using Emerald.Services.Logging;

namespace HolidayApp.Middleware;

public class StopwatchMiddleware(ILogger logger) : IMiddleware
{
    public IMiddleware? Next { set; get; }

    public async Task InvokeAsync(Emerald.AppContext context)
    {
        logger.Write($"Now: {DateTime.Now}");
        Stopwatch s = new();
        s.Start();

        if (Next != null)
            await Next.InvokeAsync(context);

        s.Stop();
        logger.Write($"Ms: {s.Elapsed.TotalMilliseconds}");
    }
}
