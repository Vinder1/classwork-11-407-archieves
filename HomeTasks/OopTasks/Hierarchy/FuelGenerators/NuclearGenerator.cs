namespace Hierarchy.FuelGenerators;

public class NuclearGenerator : FuelGenerator
{
    protected override int TurnFuelIntoEnergy()
    {
        var toConsume =  Math.Min(15, FuelLeft);
        FuelLeft -= toConsume;
        var produced = 10 * toConsume;
        Console.WriteLine($"[#] Делим атомы...+{produced}");
        return produced;
    }

    protected override void DisposeWaste()
    {
        Console.WriteLine("[#] Аккуратно избавляемся от ядерных отходов...");
    }
}