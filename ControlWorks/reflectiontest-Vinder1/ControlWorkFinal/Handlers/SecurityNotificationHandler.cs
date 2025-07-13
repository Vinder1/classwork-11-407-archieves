using EventBus.Events;

namespace EventBus.Handlers;

public class SecurityNotificationHandler :
    IEventHandler<UserEnteredEvent>, 
    IEventFilter<UserEnteredEvent>, 
    IOrderedEventHandler<UserEnteredEvent>
{
    public void Handle(UserEnteredEvent evt)
    {
        Console.WriteLine($"{evt.Username} проник внутрь!");
    }

    public bool ShouldProcess(UserEnteredEvent evt)
    {
        return evt.Role == "Visitor";
    }

    public int Order => 1;
}