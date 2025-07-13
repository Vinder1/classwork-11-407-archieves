namespace Structures.FuelGenerators;

public partial class FuelGenerator : IEnergyGenerate, IMaintainable, IRefillable, IWasteProducer
{
    public int FuelLeft => _fuelContainer.Count;
    public bool RefillRequired => FuelLeft <= 0;

    public FuelGenerator AddFuel(string fuel, int count = 1)
    {
        Refill(fuel, count);
        return this;
    }

    public void Refill(string fuel, int count = 1)
    {
        if (!FuelLibrary.Registred(fuel)) 
            throw new Exception("Fuel doesn't exist");
            
        if (FuelLibrary.Get(fuel).Type != _energyTransformer.FuelType)
            throw new Exception("This type of fuel is not supported by this generator");

        for (var i = 0; i < count; i++)
        {
            _fuelContainer.Add(fuel);
        }
    }
    
    public int Generate()
    {
        if (_fuelContainer.Count == 0)
            return 0;
        
        var energy = _energyTransformer.TurnFuelIntoEnergy(_fuelContainer[^1]);
        _fuelContainer.RemoveAt(_fuelContainer.Count - 1);

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
    public void Serve() => Refill("coal", 5); //buying some fuel
    
    private readonly IFuelToEnergyTransformer _energyTransformer;
    private readonly IWasteDisposer _wasteDisposer;
    private readonly List<string> _fuelContainer = [];
}