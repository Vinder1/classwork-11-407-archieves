namespace PapersPlease;

public interface IHandler<T>
{
    IHandler<T>? Successor { get; set; }
    void Handle(T element);
}