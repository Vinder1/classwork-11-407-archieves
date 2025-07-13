namespace Observer;

class Program
{
    static void Main(string[] args)
    {
        var forecastStation = new ForecastStation();
        forecastStation.AddObserver(new ForecastObserver());
        
        forecastStation.NotifyObservers(11.0);
    }
}