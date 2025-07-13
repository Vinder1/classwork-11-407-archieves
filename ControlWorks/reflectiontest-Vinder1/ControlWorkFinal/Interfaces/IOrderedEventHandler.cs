namespace EventBus;

public interface IOrderedEventHandler<T> : IEventHandler<T>
{
    public int Order { get; }   
}