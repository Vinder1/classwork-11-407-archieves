using EventBus.Events;
using EventBus.Handlers;

namespace EventBus;

class Program
{
    static void Main(string[] args)
    {
        var bus = new EventBus();
        bus.Publish(new UserEnteredEvent
        {
            Username = "Ivanov",
            Role = "Visitor"
        });
        
        bus.Publish(new AccessDeniedEvent()
        {
            Username = "Egor",
            Reason = "Banned for being Egor"
        });
        
        bus.Publish(new UserEnteredEvent
        {
            Username = "Ivanov",
            Role = "Admin"
        });
    }
}