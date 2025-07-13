namespace Observer;

public class ForecastObserver : IObserver<double>
{
    public void Update(double value)
    {
        Console.WriteLine($"Omagad new value is {value}");
    }
}