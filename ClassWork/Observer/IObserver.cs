namespace Observer;

public interface IObserver<in T>
{
    public void Update(T value);
}