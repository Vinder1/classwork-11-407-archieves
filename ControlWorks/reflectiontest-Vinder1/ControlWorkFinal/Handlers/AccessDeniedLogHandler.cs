using EventBus.Events;

namespace EventBus.Handlers;

public class AccessDeniedLogHandler : LogHandler<AccessDeniedEvent>
{
    protected override string EventToString(AccessDeniedEvent evt)
    {
        return $"[{DateTime.Now}] Access denied! | Username: {evt.Username}, Reason: {evt.Reason}";
    }
}