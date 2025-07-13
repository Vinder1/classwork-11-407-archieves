namespace Reflextion;

public class FuelEngine : IEngine
{
    public void Run()
    {
        Console.WriteLine("- Движок сжигает топливо и приводит механизм в движение");
    }

    public FuelEngine()
    {
        Console.WriteLine("Топливный движок создан!");
    }
}