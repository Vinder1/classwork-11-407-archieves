
namespace Emerald.Services.Logging;

public class ConsoleLogger : ILogger
{
    private readonly Lock _lock = new();
    
    public void StartRequestProcessing(AppContext context)
    {
        lock (_lock)
            Console.WriteLine($"[{context.Request.Url}]");
    }

    public void Write(string message)
    {
        lock(_lock)
            Console.WriteLine($"- {message}");
    }
}
