using Polymorphism.FuelGenerators;

namespace Polymorphism;

internal class Program
{
    private static void Main(string[] args)
    {
        //Console.WriteLine(SolarPanel.GetCurrentEfficiency());

        List<IEnergyGenerate> generators = [
            new CoalGenerator(),
            new SolarPanel(),
            new NuclearGenerator()
        ];

        var totalEnergy = GenerateFromMultipleSources(generators);
        
        var solarPanel = new SolarPanel();
        var petroleumGenerator = new PetroleumGenerator();
        totalEnergy += GenerateFromMultipleSources(petroleumGenerator, solarPanel);
        
        Console.WriteLine($"Total energy: {totalEnergy}");
        
        
        //Find out if generators are efficient
        Console.WriteLine($"Солнечная панель эффективна: {solarPanel.IsEfficient()}");
        Console.WriteLine($"Нефтяной генератор эффективен: {petroleumGenerator.IsEfficient()}");
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