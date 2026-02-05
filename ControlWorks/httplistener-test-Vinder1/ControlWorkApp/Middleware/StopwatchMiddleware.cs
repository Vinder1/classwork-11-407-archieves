using System.Diagnostics;
using Emerald.Middleware;

namespace ControlWorkApp.Middleware
{
    public class StopwatchMiddleware : IMiddleware
    {
        public IMiddleware? Next { set; private get; }

        public async Task InvokeAsync(Emerald.AppContext context)
        {
            Stopwatch s = new ();
            s.Start();

            if (Next != null)
                await Next.InvokeAsync(context);

            s.Stop();
            Console.WriteLine($"[{context.Request.Url}]\n - Ms: {s.Elapsed.TotalMilliseconds}");
        }
    }
}