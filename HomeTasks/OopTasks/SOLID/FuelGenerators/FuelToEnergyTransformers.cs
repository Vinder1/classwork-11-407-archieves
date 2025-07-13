namespace SOLID.FuelGenerators;

public interface IFuelToEnergyTransformer
{
    int TurnFuelIntoEnergy(ref int fuel);
}

public class CoalTransformer : IFuelToEnergyTransformer
{
    public int TurnFuelIntoEnergy(ref int fuel)
    {
        if (fuel < 0)
            return 0;
        Console.WriteLine("[#] Сжигаем уголь...+3");
        fuel -= 1;
        return 3;
    }
}

public class PetroleumTransformer : IFuelToEnergyTransformer
{
    public int TurnFuelIntoEnergy(ref int fuel)
    {
        var toConsume =  Math.Min(3, fuel);
        fuel -= toConsume;
        var produced = 4 * toConsume;
        Console.WriteLine($"[#] Сжигаем нефть...+{produced}");
        return produced;
    }
}

public class NuclearTransformer : IFuelToEnergyTransformer
{
    public int TurnFuelIntoEnergy(ref int fuel)
    {
        var toConsume =  Math.Min(15, fuel);
        fuel -= toConsume;
        var produced = 10 * toConsume;
        Console.WriteLine($"[#] Делим атомы...+{produced}");
        return produced;
    }
}