namespace EventBus.Events;

public class UserEnteredEvent : IEvent
{
    public required string Username { get; init; }
    public required string Role { get; init; }
    public DateTime Time { get; } = DateTime.Now;
}