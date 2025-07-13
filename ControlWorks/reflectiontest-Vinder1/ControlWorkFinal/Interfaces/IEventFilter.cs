namespace EventBus;

public interface IEventFilter<T>
{
    public bool ShouldProcess(T evt);
}