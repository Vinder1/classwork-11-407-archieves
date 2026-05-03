namespace AspNetApp.DomainUtility;

public static class Angle
{
    /// <summary>
    /// Нормализует угол в диапазон [0, 2π)
    /// </summary>
    public static double Normalize(double angle)
    {
        while (angle < 0)
            angle += 2 * Math.PI;
        while (angle >= 2 * Math.PI)
            angle -= 2 * Math.PI;
        return angle;
    }
}