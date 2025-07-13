namespace Reflextion;

class Program
{
    static void Main(string[] args)
    {
        DependencyInjector.Register<IHuman, Sportsman>();
        DependencyInjector.Register<ICar, BigCar>();
        DependencyInjector.Register<IEngine, FuelEngine>();
        
        Console.WriteLine(DependencyInjector.Resolve<IHuman>().GetType());
        Console.WriteLine(DependencyInjector.Resolve<ICar>().GetType());
        Console.WriteLine(DependencyInjector.Resolve<IEngine>().GetType());

        try
        {
            Console.WriteLine(DependencyInjector.Resolve<string>().GetType());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}