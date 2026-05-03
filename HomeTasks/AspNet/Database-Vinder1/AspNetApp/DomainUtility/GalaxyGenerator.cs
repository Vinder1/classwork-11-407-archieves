using AspNetApp.Entity;

namespace AspNetApp.DomainUtility;

public static class GalaxyGenerator
{
    public static Galaxy CreateGalaxy(int version, Star[][] stars) => new()
    {
        Version=version, Stars=new StarCollection(stars) 
    };

    public static Star[][] GenerateStars(
        int maxRadius = DefaultGalaxyParameters.MaxRadius,
        double density = DefaultGalaxyParameters.Density,
        int arms = DefaultGalaxyParameters.Arms)
    {
        Star[][] stars = new Star[maxRadius + 1][];
        var comparer = new StarAngleComparer();

        for (int R = 1; R <= maxRadius; R++)
        {
            int N = Math.Max(6, (int)(density * R));
            stars[R] = new Star[N];

            double baseAngleStep = 2 * Math.PI / (N+1);

            for (int i = 0; i < N; i++)
            {
                // Базовая спиральная структура
                double armAngle = i * baseAngleStep;

                // Добавляем принадлежность к рукавам
                double spiralOffset = 0.5 * Math.Log(R) * Math.Sin(armAngle * arms + R * 0.3);

                double angle = armAngle + spiralOffset + NextGaussian(0, 0.1);

                angle = Angle.Normalize(angle);

                stars[R][i] = new Star
                {
                    AngleMilliradians = Angle.RadiansToMilliradians(angle),
                    Radius = R
                };
            }

            Array.Sort(stars[R], comparer);
            var minDiff = (int) (100 / Math.Log(R+1));
            
            for (var i = 1; i < N; i++)
            {
                if (stars[R][i].AngleMilliradians < stars[R][i-1].AngleMilliradians + minDiff)
                {
                    stars[R][i] = stars[R][i] with
                    {
                        AngleMilliradians = stars[R][i-1].AngleMilliradians + minDiff
                    };
                }
            }
            int counter = 0;
            for (int i = N-1; i > 0 && stars[R][i].AngleMilliradians >= Angle.TwoPiMilliradians; i--)
            {
                counter++;
            }
            if (counter > 0)
            {
                Array.Resize(ref stars[R], N-counter);
            }
        }
        return stars;
    }

    private static double NextGaussian(double mean, double stdDev)
    {
        double u1 = 1.0 - Random.Shared.NextDouble();
        double u2 = 1.0 - Random.Shared.NextDouble();

        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

        return mean + stdDev * randStdNormal;
    }

    public class StarAngleComparer : IComparer<Star>
    {
        public int Compare(Star x, Star y)
        {
            return x.AngleMilliradians.CompareTo(y.AngleMilliradians);
        }
    }
}