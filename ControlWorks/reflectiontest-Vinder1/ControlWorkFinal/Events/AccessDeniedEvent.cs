namespace EventBus.Events;

public class AccessDeniedEvent : IEvent
{
    public required string Username { get; init; }
    public required string Reason { get; init; }
}