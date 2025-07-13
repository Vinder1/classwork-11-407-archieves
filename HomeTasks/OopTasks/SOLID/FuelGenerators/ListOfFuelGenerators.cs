namespace SOLID.FuelGenerators;

public partial class FuelGenerator
{
    public static FuelGenerator Coal()
        => new FuelGenerator(new CoalTransformer(), new DoingNothingDisposer());

    public static FuelGenerator Petroleum()
        => new FuelGenerator(new PetroleumTransformer(), new DoingNothingDisposer());
    
    public static FuelGenerator EcoFriendlyPetroleum()
        => new FuelGenerator(new PetroleumTransformer(), new CarefulDisposer());
    
    public static FuelGenerator Nuclear()
        => new FuelGenerator(new NuclearTransformer(), new CarefulDisposer());
    
    /// <summary>
    /// OMAGAD YOU ARE BRAVE ENOUGH TO USE IT?
    /// </summary>
    public static FuelGenerator UnstableNuclear()
        => new FuelGenerator(new NuclearTransformer(), new DoingNothingDisposer());
}