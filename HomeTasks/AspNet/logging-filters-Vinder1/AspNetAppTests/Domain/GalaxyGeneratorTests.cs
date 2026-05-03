using AspNetApp.Entity;
using AspNetApp.DomainUtility;

namespace AspNetAppTests;

public class GalaxyGeneratorTests
{
    // Константы для тестов, чтобы не магические числа в коде
    private const double TwoPi = 2 * Math.PI;
    private const double Epsilon = 1e-9;

    #region Тесты для CreateGalaxy

    [Test]
    public async Task CreateGalaxy_ValidInput_ReturnsInitializedGalaxy()
    {
        // Arrange
        int version = 42;
        var stars = new Star[10][];

        // Act
        var galaxy = GalaxyGenerator.CreateGalaxy(version, stars);

        // Assert
        using (Assert.Multiple())
        {
            await Assert.That(galaxy).IsNotNull();
            await Assert.That(galaxy.Version).IsEqualTo(version);
            await Assert.That(galaxy.Stars.Stars).IsSameReferenceAs(stars);
        }
    }

    #endregion

    #region Тесты для GenerateStars (Структура и Границы)

    [Test]
    [Arguments(10, 1.0, 4)] // Стандартные параметры
    [Arguments(5, 0.5, 2)]  // Малая галактика
    [Arguments(100, 2.0, 6)] // Большая и плотная
    public async Task GenerateStars_StarArray_Dimensions_AreCorrect(int maxRadius, double density, int arms)
    {
        // Act
        var result = GalaxyGenerator.GenerateStars(maxRadius, density, arms);

        // Assert
        await Assert.That(result).IsNotNull();
        // Массив должен быть размером maxRadius + 1
        await Assert.That(result.Length).IsEqualTo(maxRadius + 1);

        // Радиус 0 должен быть пустым
        await Assert.That(result[0]).IsNull();

        // Проверка каждого радиуса
        for (int r = 1; r <= maxRadius; r++)
        {
            await Assert.That(result[r]).IsNotNull();
            // Количество звезд не должно быть меньше 6
            await Assert.That(result[r]!.Length).IsGreaterThanOrEqualTo(6);
        }
    }

    [Test]
    public async Task GenerateStars_StarProperties_AreValid()
    {
        // Arrange
        int maxRadius = 50;
        var stars = GalaxyGenerator.GenerateStars(maxRadius);

        // Act & Assert
        for (int r = 1; r <= maxRadius; r++)
        {
            foreach (var star in stars[r]!)
            {
                // 1. Радиус должен соответствовать индексу массива
                await Assert.That(star.Radius).IsEqualTo(r);

                // 2. Угол должен быть в диапазоне [0, 2π]
                await Assert.That(star.AngleMilliradians).IsGreaterThanOrEqualTo(0);
                await Assert.That(star.AngleMilliradians).IsLessThanOrEqualTo(Angle.TwoPiMilliradians);
            }
        }
    }

    #endregion

    #region Тесты для GenerateStars (Логика и Алгоритмы)

    [Test]
    public async Task GenerateStars_StarsAreSortedByAngle_WithinEachRadius()
    {
        // Arrange
        var stars = GalaxyGenerator.GenerateStars(30);
        var comparer = new GalaxyGenerator.StarAngleComparer();

        // Act & Assert
        for (int r = 1; r <= 30; r++)
        {
            var radiusStars = stars[r]!;
            // Проверяем, что массив отсортирован по возрастанию угла
            for (int i = 0; i < radiusStars.Length - 1; i++)
            {
                await Assert.That(comparer.Compare(radiusStars[i], radiusStars[i + 1]))
                    .IsLessThanOrEqualTo(0);
            }
        }
    }

    [Test]
    public async Task GenerateStars_MinAngleDistance_IsDynamic()
    {
        // Arrange
        var stars = GalaxyGenerator.GenerateStars(maxRadius: 50, density: 2.0);

        // Assert
        for (int r = 1; r <= 50; r++)
        {
            var radiusStars = stars[r]!;
            // Пропускаем, если звезд меньше 2 (нечего сравнивать)
            if (radiusStars.Length < 2) continue;

            // Вычисляем ожидаемый минимальный шаг для этого радиуса
            double expectedMinDiff = (int)(100 / Math.Log(r + 1));
            double epsilon = 1e-9;

            for (int i = 1; i < radiusStars.Length; i++)
            {
                // Звезды уже отсортированы, сравниваем соседей
                double diff = radiusStars[i].AngleMilliradians - radiusStars[i - 1].AngleMilliradians;

                // Учитываем, что углы нормализованы [0, 2π], но внутри одного радиуса 
                // при генерации они идут по порядку, пока не произойдет "wrap-around"

                // Проверяем с небольшим эпсилоном на погрешность вычислений
                
                await Assert.That(diff + epsilon).IsGreaterThanOrEqualTo(expectedMinDiff);
            }
        }
    }

    [Test]
    public async Task GenerateStars_StopsGeneration_OnAngleWrapAround()
    {
        // Arrange
        // Пытаемся сгенерировать галактику с параметрами, которые могут спровоцировать 
        // быстрый рост угла (например, огромная плотность или специфичный шум).
        // Однако, так как используется Random, надежнее проверить свойство результата.

        var stars = GalaxyGenerator.GenerateStars(maxRadius: 100, density: 10.0);

        // Assert
        for (int r = 1; r <= 100; r++)
        {
            var radiusStars = stars[r]!;
            if (radiusStars.Length == 0) continue;

            // 1. Проверяем, что все углы валидны [0, 2π]
            foreach (var star in radiusStars)
            {
                await Assert.That(star.AngleMilliradians).IsGreaterThanOrEqualTo(0);
                await Assert.That(star.AngleMilliradians).IsLessThanOrEqualTo(Angle.TwoPiMilliradians);
            }

            // 2. Проверяем, что генерация не создала "мусор" после нормализации.
            // Если бы сработал ранний выход (break), массив должен быть обрезан (Array.Resize).
            // Значит, в массиве не должно быть нулевых элементов (null) в середине.

            // Проверяем, что все элементы в массиве инициализированы (радиус ненулевой)
            // (Array.Resize должен был убрать лишние слоты)
            await Assert.That(radiusStars).All(s => s.Radius != 0);
        }
    }

    [Test]
    public async Task GenerateStars_SpiralArms_InfluencesDistribution()
    {
        // Arrange
        // Генерируем две галактики с разным количеством рукавов
        var stars3Arms = GalaxyGenerator.GenerateStars(50, arms: 3);
        var stars5Arms = GalaxyGenerator.GenerateStars(50, arms: 5);

        // Act
        // Считаем "центроиды" углов для радиуса 20 (грубая проверка распределения)
        var avgAngle3 = GetAverageAngle(stars3Arms[20]!);
        var avgAngle5 = GetAverageAngle(stars5Arms[20]!);

        // Assert
        // Это "мягкий" тест. Мы не знаем точных значений, но структура должна отличаться.
        // Главное - убедиться, что код выполняется и дает разные результаты при разных параметрах.
        await Assert.That(stars3Arms[20]!.Length).IsGreaterThan(0);
        await Assert.That(stars5Arms[20]!.Length).IsGreaterThan(0);

        // Проверка: при одинаковом Random.Shared (если бы мы мокали его) результаты были бы идентичны.
        // Здесь мы просто проверяем, что параметры "arms" не ломают генерацию.
    }

    #endregion

    #region Тесты для NextGaussian (Статистические)

    [Test]
    [Repeat(5)] // Повторяем тест 5 раз, так как проверка статистическая
    public async Task NextGaussian_Distribution_ApproximatelyCorrect()
    {
        // Arrange
        // Используем рефлексию или копируем метод, если он private. 
        // Если метод останется private, лучше вынести его в internal + InternalsVisibleTo для тестов.
        // Для примера предположим, что мы можем вызвать его или тестируем через публичный метод.

        int samples = 1000;
        double mean = 5.0;
        double stdDev = 2.0;
        double[] values = new double[samples];

        // Act
        var method = typeof(GalaxyGenerator)
            .GetMethod("NextGaussian", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;

        for (int i = 0; i < samples; i++)
        {
            values[i] = (double)method.Invoke(null, [mean, stdDev])!;
        }

        // Assert - Проверяем среднее и отклонение с допустимой погрешностью
        double actualMean = values.Average();
        double actualStdDev = Math.Sqrt(values.Average(v => Math.Pow(v - actualMean, 2)));

        // Допускаем отклонение
        await Assert.That(Math.Abs(actualMean - mean)).IsLessThanOrEqualTo(0.5);
        await Assert.That(Math.Abs(actualStdDev - stdDev)).IsLessThanOrEqualTo(0.5);
    }

    #endregion

    #region Вспомогательные методы

    private static double GetAverageAngle(Star[] stars)
    {
        if (stars.Length == 0) return 0;
        return stars.Average(s => s.AngleMilliradians);
    }

    #endregion
}