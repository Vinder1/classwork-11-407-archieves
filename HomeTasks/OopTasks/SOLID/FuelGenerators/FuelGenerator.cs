namespace SOLID.FuelGenerators;

public partial class FuelGenerator : IEnergyGenerate, IMaintainable, IRefillable, IWasteProducer
{
    public int FuelLeft => _fuelLeft;
    public bool RefillRequired => FuelLeft <= 0;
    private int _fuelLeft = 100;

    public void Refill(int amount)
    {
        if (amount <= 0)
            return;
        _fuelLeft += amount;
    }
    
    public int Generate()
    {
        if (FuelLeft <= 0)
            return 0;
        var energy = _energyTransformer.TurnFuelIntoEnergy(ref _fuelLeft);
        ProduceWaste();
        _wasteDisposer.DisposeWaste();
        return energy;
    }

    private FuelGenerator(IFuelToEnergyTransformer transformer, IWasteDisposer disposer)
    {
        _energyTransformer = transformer;
        _wasteDisposer = disposer;
    }

    public void ProduceWaste()
    {
        //....producing waste
    }
    
    public bool MaintenanceRequired() => RefillRequired;
    public void Serve() => Refill(100); //buying some fuel
    
    private readonly IFuelToEnergyTransformer _energyTransformer;
    private readonly IWasteDisposer _wasteDisposer;
}