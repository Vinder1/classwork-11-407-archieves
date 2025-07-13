namespace Structures.FuelGenerators;

public partial class FuelGenerator
{
    public static FuelGenerator Solid()
        => new FuelGenerator(new SolidFuelTransformer(), new DoingNothingDisposer());

    public static FuelGenerator Liquid()
        => new FuelGenerator(new LiquidFuelTransformer(), new DoingNothingDisposer());
    
    public static FuelGenerator EcoFriendlyLiquid()
        => new FuelGenerator(new LiquidFuelTransformer(), new CarefulDisposer());
    
    public static FuelGenerator Nuclear()
        => new FuelGenerator(new NuclearTransformer(), new CarefulDisposer());
    
    /// <summary>
    /// OMAGAD YOU ARE BRAVE ENOUGH TO USE IT?
    /// </summary>
    public static FuelGenerator UnstableNuclear()
        => new FuelGenerator(new NuclearTransformer(), new DoingNothingDisposer());
}