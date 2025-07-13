namespace PapersPlease;

public class HandlersSequence<T>
{
    private IHandler<T>? firstHandler;
    private IHandler<T>? lastHandler;
        
    public HandlersSequence()
    {
    }

    public void AddHandler(IHandler<T> handler)
    {
        if (firstHandler == null)
        {
            firstHandler = lastHandler = handler;
            return;
        }
        
        lastHandler!.Successor = handler;
        lastHandler = handler;
    }

    public void Handle(T element)
    {
        firstHandler?.Handle(element);
    }
}