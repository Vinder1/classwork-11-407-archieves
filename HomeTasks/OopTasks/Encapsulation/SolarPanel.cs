namespace Encapsulation;

public class SolarPanel : IEnergyGenerate
{
    public int Generate()
    {
        Console.WriteLine("[#] Благодарим солнышко за энергию...");
        return (int)(10 * GetCurrentEfficiency());
    }
    
    
    /// <summary>
    /// Return coefficient of sun efficiency; 1.0 at noon, 0.0 at midnight, 0.66 at 8 AM, 0.5 at 6 AM...
    /// </summary>
    public static double GetCurrentEfficiency() => 1.0 - Math.Abs(Now - Noon) / Noon;
    
    private const int Noon = 12*60*60;
    private static double Now => (int)DateTime.Now.TimeOfDay.TotalSeconds;
}