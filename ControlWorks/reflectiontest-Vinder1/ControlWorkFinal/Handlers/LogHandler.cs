using System.Text;

namespace EventBus.Handlers;

public abstract class LogHandler<T> : IEventHandler<T> where T : IEvent
{
    public void Handle(T evt)
    {
        lock (locker)
        {
            using var writer = new FileStream("log.txt", FileMode.Append);
            writer.Write(Encoding.UTF8.GetBytes(EventToString(evt)));
            writer.Write("\n"u8);
        }
    }

    private static object locker = new object();
    
    
    protected abstract string EventToString(T evt);
}