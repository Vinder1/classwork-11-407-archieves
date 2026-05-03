using AspNetApp.DomainUtility;

namespace AspNetApp.Entity;

public class StarCollection(Star[][] stars) : IStarCollection
{
    public Star[][] Stars { get; } = stars;
    private const double E = 1e-7; //погрешность сравнения

    public Star? Find(int radius, int angleMilliradians)
    {
        var searchGroup = Stars[radius];
        var left = 0;
        var right = searchGroup.Length;

        while (right - left > 1)
        {
            int mid = (right + left) / 2;

            if (searchGroup[mid].AngleMilliradians == angleMilliradians)
                return searchGroup[mid];

            if (searchGroup[mid].AngleMilliradians < angleMilliradians)
                left = mid;
            else
                right = mid;
        }

        if (searchGroup[left].AngleMilliradians == angleMilliradians)
            return searchGroup[left];
        else
            return null;
    }

    public IEnumerable<Star> GetAllStars()
    {
        return Stars.Where(s => s != null).SelectMany(s => s);
    }

    public IEnumerable<Star> FindNearbyStars(int sourceRadius, int sourceAngleMilliradians, int radius)
    {
        var result = new List<Star>();

        for (var targetRadius = Math.Max(1, sourceRadius - radius); 
            targetRadius < Math.Min(sourceRadius + radius, Stars.Length - 1); targetRadius++)
        {
            result.AddRange(FindStarsInRadiusRange(targetRadius, sourceRadius, sourceAngleMilliradians));
        }

        return result;
    }

    /// <summary>
    /// Находит звезды в указанном радиусе, которые находятся на расстоянии не более 2
    /// от точки с заданным радиусом и углом
    /// </summary>
    private List<Star> FindStarsInRadiusRange(int targetRadius, int sourceRadius, int sourceAngleMilliradians)
    {
        if (targetRadius < 0 || targetRadius >= Stars.Length)
            return [];

        // Рассчитываем минимальный и максимальный угол для соседнего радиуса
        var (minAngleMil, maxAngleMil) = CalculateAngleRange(sourceRadius, sourceAngleMilliradians, targetRadius);

        // Нормализуем углы
        minAngleMil = Angle.NormalizeMilliradians(minAngleMil);
        maxAngleMil = Angle.NormalizeMilliradians(maxAngleMil);

        var starsInRadius = Stars[targetRadius];
        var result = new List<Star>();

        // Если диапазон не пересекает границу 0/2π
        if (minAngleMil <= maxAngleMil)
        {
            var startIndex = FindFirstStarIndex(starsInRadius, minAngleMil);
            if (startIndex == -1) return result;

            for (int i = startIndex; i < starsInRadius.Length; i++)
            {
                var star = starsInRadius[i];
                if (star.AngleMilliradians > maxAngleMil) break;

                if (IsWithinDistance(star, sourceRadius, sourceAngleMilliradians))
                    result.Add(star);
            }
        }
        else
        {
            // Диапазон пересекает границу - ищем в двух интервалах
            // Интервал 1: [minAngle, 2π)
            var startIndex1 = FindFirstStarIndex(starsInRadius, minAngleMil);
            if (startIndex1 != -1)
            {
                for (int i = startIndex1; i < starsInRadius.Length; i++)
                {
                    var star = starsInRadius[i];
                    if (IsWithinDistance(star, sourceRadius, sourceAngleMilliradians))
                        result.Add(star);
                }
            }

            // Интервал 2: [0, maxAngle]
            var endIndex2 = FindLastStarIndex(starsInRadius, maxAngleMil);
            if (endIndex2 != -1)
            {
                for (int i = 0; i <= endIndex2; i++)
                {
                    var star = starsInRadius[i];
                    if (IsWithinDistance(star, sourceRadius, sourceAngleMilliradians))
                        result.Add(star);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Рассчитывает минимальный и максимальный угол для targetRadius,
    /// при котором расстояние между точками (sourceRadius, sourceAngle) и (targetRadius, targetAngle) ≤ 2
    /// </summary>
    private static (int minAngle, int maxAngle) CalculateAngleRange(int sourceRadius, int sourceAngleMilliradians, int targetRadius)
    {
        const double maxDistance = 2.0;

        var sourceAngle = Angle.MilliradiansToRadians(sourceAngleMilliradians);
        double r1 = sourceRadius;
        double r2 = targetRadius;

        // По теореме косинусов: d² = r1² + r2² - 2*r1*r2*cos(Δθ)
        // maxDistance² = r1² + r2² - 2*r1*r2*cos(Δθ)
        // cos(Δθ) = (r1² + r2² - maxDistance²) / (2*r1*r2)

        double minCos = (r1 * r1 + r2 * r2 - maxDistance * maxDistance) / (2 * r1 * r2);

        // Ограничиваем значение в пределах [-1, 1] из-за погрешностей вычислений
        minCos = Math.Max(-1, Math.Min(1, minCos));

        // Максимальная разница углов
        double maxDeltaAngle = Math.Acos(minCos);

        double minAngle = sourceAngle - maxDeltaAngle;
        double maxAngle = sourceAngle + maxDeltaAngle;

        return (Angle.RadiansToMilliradians(minAngle), Angle.RadiansToMilliradians(maxAngle));
    }

    /// <summary>
    /// Проверяет, находится ли звезда на расстоянии ≤ 2 от заданной точки
    /// </summary>
    private static bool IsWithinDistance(Star star, int sourceRadius, int sourceAngleMilliradians)
    {
        double r1 = sourceRadius;
        double r2 = star.Radius;
        var angleMilliradians = Math.Abs(star.AngleMilliradians - sourceAngleMilliradians);
        double deltaAngle = Angle.MilliradiansToRadians(Angle.NormalizeMilliradians(angleMilliradians));
        deltaAngle = Math.Min(deltaAngle, 2 * Math.PI - deltaAngle);

        double distance = Math.Sqrt(r1 * r1 + r2 * r2 - 2 * r1 * r2 * Math.Cos(deltaAngle));
        return distance <= 2.0 + E;
    }

    /// <summary>
    /// Находит индекс первого элемента с углом >= target
    /// </summary>
    private static int FindFirstStarIndex(Star[] stars, int targetAngleMilliradians)
    {
        int left = 0;
        int right = stars.Length - 1;

        while (right - left > 1)
        {
            int mid = (left + right) / 2;

            if (stars[mid].AngleMilliradians >= targetAngleMilliradians)
            {
                right = mid;
            }
            else
            {
                left = mid;
            }
        }

        return right;
    }

    /// <summary>
    /// Находит индекс последнего элемента с углом <= target
    /// </summary>
    private static int FindLastStarIndex(Star[] stars, int targetAngleMilliradians)
    {
        int left = 0;
        int right = stars.Length - 1;

        while (right - left > 1)
        {
            int mid = (left + right) / 2;

            if (stars[mid].AngleMilliradians <= targetAngleMilliradians)
            {
                left = mid;
            }
            else
            {
                right = mid;
            }
        }

        return left;
    }
}