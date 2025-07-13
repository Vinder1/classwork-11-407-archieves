namespace Polymorphism.FuelGenerators;

public class CoalGenerator : FuelGenerator
{
    protected override int TurnFuelIntoEnergy()
    {
        Console.WriteLine("[#] Сжигаем уголь...+3");
        FuelLeft -= 1;
        return 3;
    }
}