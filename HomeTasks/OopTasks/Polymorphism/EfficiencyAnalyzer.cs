namespace Polymorphism;

public static class EfficiencyAnalyzer
{
    public static bool IsEfficient(this SolarPanel panel) => SolarPanel.GetCurrentEfficiency() > 0.5;
    
    public static bool IsEfficient(this FuelGenerator fuelGenerator) => fuelGenerator.FuelLeft > 0;
}