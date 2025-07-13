namespace PapersPlease;

public abstract class BaseHandler<T> : IHandler<T>
{
    protected BaseHandler(IHandler<T>? successor)
    {
        Successor = successor;
    }
    
    protected BaseHandler()
    {
    }

    public IHandler<T>? Successor { get; set; }
    public abstract void Handle(T element);
}