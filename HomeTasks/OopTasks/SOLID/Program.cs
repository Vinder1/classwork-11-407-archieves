using SOLID.FuelGenerators;

namespace SOLID;

internal class Program
{
    private static void Main(string[] args)
    {
        List<IEnergyGenerate> generators =
        [
            new SolarPanel(),
            FuelGenerator.UnstableNuclear(),
            FuelGenerator.EcoFriendlyPetroleum()
        ];

        var energy = 0;
        foreach (var generator in generators)
        {
            if (generator is IMaintainable maintainable && maintainable.MaintenanceRequired())
            {
                maintainable.Serve();
            }
            
            energy += generator.Generate();
        }
        
        Console.WriteLine(energy);
    }
}