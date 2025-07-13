namespace Observer;

public class ForecastStation: IObservable<double>
{
    private List<IObserver<double>> observers = [];
    
    public void AddObserver(IObserver<double> observer)
    {
        observers.Add(observer);
    }

    public void NotifyObservers(double value)
    {
        foreach (var observer in observers)
        {
            observer.Update(value);
        }
    }

    public void RemoveObserver(IObserver<double> observer)
    {
        observers.Remove(observer);
    }
}