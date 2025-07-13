namespace DwarfFortressObserver;

public interface IDelegateObservable<in T>
{
    public void NotifyObservers(T arg);
}