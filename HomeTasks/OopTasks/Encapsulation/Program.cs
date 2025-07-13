using Encapsulation.FuelGenerators;

namespace Encapsulation;

internal class Program
{
    private static void Main(string[] args)
    {
        var uberMegaGenerator = new NuclearGenerator();

        //uberMegaGenerator.DisposeWaste(); // No access
        //uberMegaGenerator.TurnFuelIntoEnergy(); // No access
        //SolarPanel.Noon // No access
        
        Console.WriteLine($"Топлива в мегагенераторе: {uberMegaGenerator.FuelLeft}");
        // uberMegaGenerator.FuelLeft -= 10; // Нельзя просто так украсть((((
        uberMegaGenerator.AddFuel(-10);
        Console.WriteLine($"Топлива в мегагенераторе (после попытки украсть): {uberMegaGenerator.FuelLeft}");
        uberMegaGenerator.AddFuel(10);
        Console.WriteLine($"Топлива в мегагенераторе (после добавления нового): {uberMegaGenerator.FuelLeft}");
    }

    private static int GenerateFromMultipleSources(List<IEnergyGenerate> generators)
    {
        var totalEnergy = 0;
        foreach (var generator in generators)
            totalEnergy += generator.Generate();
        return totalEnergy;
    }
    
    private static int GenerateFromMultipleSources(IEnergyGenerate firstGenerator, IEnergyGenerate secondGenerator)
    {
        return firstGenerator.Generate() + secondGenerator.Generate();
    }
}