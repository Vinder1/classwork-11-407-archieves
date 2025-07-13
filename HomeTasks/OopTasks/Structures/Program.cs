using Structures.FuelGenerators;

namespace Structures;

internal class Program
{
    private static void Main(string[] args)
    {
        RegisterFuels();
        
        List<IEnergyGenerate> generators =
        [
            new SolarPanel(),
            FuelGenerator.UnstableNuclear()
                .AddFuel("plutonium"),
            FuelGenerator.EcoFriendlyLiquid()
                .AddFuel("ethanol"),
            FuelGenerator.Solid()
                .AddFuel("coal"),
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
    
    private static void RegisterFuels()
    {
        FuelLibrary.RegisterFuel("coal", new FuelInfo("Уголь", FuelType.Solid, 100));
        FuelLibrary.RegisterFuel("peat", new FuelInfo("Торф", FuelType.Solid, 80));
        FuelLibrary.RegisterFuel("trash", new FuelInfo("Мусор", FuelType.Solid, 10));
        
        FuelLibrary.RegisterFuel("kerosene", new FuelInfo("Керосин", FuelType.Liquid, 200));
        FuelLibrary.RegisterFuel("diesel", new FuelInfo("Дизель", FuelType.Liquid, 150));
        FuelLibrary.RegisterFuel("ethanol", new FuelInfo("Этанол", FuelType.Liquid, 80));
        
        FuelLibrary.RegisterFuel("uranium", new FuelInfo("Уран", FuelType.Nuclear, 100));
        FuelLibrary.RegisterFuel("thorium", new FuelInfo("Торий", FuelType.Nuclear, 10000));
        FuelLibrary.RegisterFuel("plutonium", new FuelInfo("Плутоний", FuelType.Nuclear, 10000000));
    }
}