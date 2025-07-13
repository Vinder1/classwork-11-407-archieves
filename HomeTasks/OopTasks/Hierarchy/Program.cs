using Hierarchy.FuelGenerators;

namespace Hierarchy;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(new CoalGenerator().Generate());
        Console.WriteLine(new NuclearGenerator().Generate());
        Console.WriteLine(new SolarPanel().Generate());
    }
}