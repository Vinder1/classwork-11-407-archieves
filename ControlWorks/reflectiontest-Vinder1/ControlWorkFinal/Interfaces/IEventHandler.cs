namespace EventBus;

public interface IEventHandler<T>
{
    public void Handle(T evt);
}