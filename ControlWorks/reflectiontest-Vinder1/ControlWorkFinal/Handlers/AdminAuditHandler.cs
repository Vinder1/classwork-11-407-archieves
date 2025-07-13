using EventBus.Events;

namespace EventBus.Handlers;

public class AdminAuditHandler : 
    IEventHandler<UserEnteredEvent>,
    IEventFilter<UserEnteredEvent>,
    IOrderedEventHandler<UserEnteredEvent>
{
    public void Handle(UserEnteredEvent evt)
    {
        Console.WriteLine($"Админ {evt.Username} проводит аудит!");
    }

    public bool ShouldProcess(UserEnteredEvent evt)
    {
        return evt.Role == "Admin";
    }

    public int Order => 0;
}