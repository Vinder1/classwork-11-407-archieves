namespace Encapsulation;

public abstract class FuelGenerator : IEnergyGenerate
{
    public int FuelLeft { get; protected set; } = 100;

    public void AddFuel(int fuel)
    {
        if (fuel <= 0)
            return;
        FuelLeft += fuel;
    }
    
    public int Generate()
    {
        if (FuelLeft <= 0)
            return 0;
        var energy = TurnFuelIntoEnergy();
        DisposeWaste();
        return energy;
    }
    
    protected abstract int TurnFuelIntoEnergy();

    protected virtual void DisposeWaste()
    {
        Console.WriteLine("[#] Отходы? Это проблема моих внуков");
    }
}