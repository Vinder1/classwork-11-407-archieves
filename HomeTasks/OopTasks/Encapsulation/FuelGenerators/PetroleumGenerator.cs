namespace Encapsulation.FuelGenerators;

public class PetroleumGenerator : FuelGenerator
{
    protected override int TurnFuelIntoEnergy()
    {
        var toConsume =  Math.Min(3, FuelLeft);
        FuelLeft -= toConsume;
        var produced = 4 * toConsume;
        Console.WriteLine($"[#] Сжигаем нефть...+{produced}");
        return produced;
    }
}