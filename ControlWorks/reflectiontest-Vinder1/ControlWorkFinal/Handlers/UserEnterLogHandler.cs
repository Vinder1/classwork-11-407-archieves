using EventBus.Events;

namespace EventBus.Handlers;

public class UserEnterLogHandler : LogHandler<UserEnteredEvent>
{
    protected override string EventToString(UserEnteredEvent evt)
    {
        return $"[{DateTime.Now}] User entered!  | Name: {evt.Username}, Role: {evt.Role}";
    }
}