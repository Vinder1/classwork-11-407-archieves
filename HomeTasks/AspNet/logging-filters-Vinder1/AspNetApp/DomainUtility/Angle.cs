namespace AspNetApp.DomainUtility;

/// <summary>
/// Static class with utility methods for angle operations
/// </summary>
public static class Angle
{
    /// <summary>
    /// 
    /// </summary>
    public const double TwoPi = 2 * Math.PI;
    /// <summary>
    /// 
    /// </summary>
    public const int TwoPiMilliradians = (int)(TwoPi * 1000);
    /// <summary>
    /// Нормализует угол в диапазон [0, 2π)
    /// </summary>
    public static double Normalize(double angle)
    {
        while (angle < 0)
            angle += TwoPi;
        while (angle >= TwoPi)
            angle -= TwoPi;
        return angle;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static int NormalizeMilliradians(int angle)
    {
        while (angle < 0)
            angle += TwoPiMilliradians;
        while (angle >= TwoPiMilliradians)
            angle -= TwoPiMilliradians;
        return angle;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="radians"></param>
    /// <returns></returns>
    public static int RadiansToMilliradians(double radians)
    {
        return (int)Math.Round(radians * 1000);
    }

    /// <summary>
    /// Конвертирует миллирадианы в радианы
    /// </summary>
    public static double MilliradiansToRadians(int milliradians)
    {
        return milliradians / 1000.0;
    }
}