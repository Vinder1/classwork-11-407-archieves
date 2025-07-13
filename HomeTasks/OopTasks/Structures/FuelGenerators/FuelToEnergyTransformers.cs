namespace Structures.FuelGenerators;

public interface IFuelToEnergyTransformer
{
    FuelType FuelType { get; }
    int TurnFuelIntoEnergy(string fuel);
}

public class SolidFuelTransformer : IFuelToEnergyTransformer
{
    public FuelType FuelType => FuelType.Solid;
    public int TurnFuelIntoEnergy(string fuel)
    {
        var fuelData = FuelLibrary.Get(fuel);
        var produced = (int) (3 * (fuelData.EfficiencyInPercents / 100f));
        Console.WriteLine($"[#] Закидываем {fuelData.Name} в топку...+{produced}");
        return produced;
    }
}

public class LiquidFuelTransformer : IFuelToEnergyTransformer
{
    public FuelType FuelType => FuelType.Liquid;
    public int TurnFuelIntoEnergy(string fuel)
    {
        var fuelData = FuelLibrary.Get(fuel);
        var produced = (int) (8 * (fuelData.EfficiencyInPercents / 100f));
        Console.WriteLine($"[#] Заливаем {fuelData.Name} в двигатель...+{produced}");
        return produced;
    }
}

public class NuclearTransformer : IFuelToEnergyTransformer
{
    public FuelType FuelType => FuelType.Nuclear;
    public int TurnFuelIntoEnergy(string fuel)
    {
        var fuelData = FuelLibrary.Get(fuel);
        var produced = (int) (15 * (fuelData.EfficiencyInPercents / 100f));
        Console.WriteLine($"[#] Делим атомы ({fuelData.Name})...+{produced}");
        
        if (new Random().Next() % 100 < 5)
            throw new Exception("BABAHHHHHHHHH");
        
        return produced;
    }
}