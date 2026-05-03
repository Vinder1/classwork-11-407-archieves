using AspNetApp.DomainUtility;

namespace AspNetAppTests;

public class AngleTests
{
    private const double TwoPi = 2 * Math.PI;
    private const double Pi = Math.PI;
    private const double Epsilon = 1e-9;

    #region 1. Базовые случаи (уже в диапазоне)

    [Test]
    public async Task Normalize_Zero_ReturnsZero()
    {
        var result = Angle.Normalize(0);
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task Normalize_Pi_ReturnsPi()
    {
        var result = Angle.Normalize(Pi);
        await Assert.That(result).IsEqualTo(Pi);
    }

    [Test]
    public async Task Normalize_HalfPi_ReturnsHalfPi()
    {
        var result = Angle.Normalize(Pi / 2);
        await Assert.That(result).IsEqualTo(Pi / 2);
    }

    [Test]
    public async Task Normalize_ValueInsideRange_ReturnsSame()
    {
        var result = Angle.Normalize(1.5); // 1.5 радиан внутри [0, 2π)
        await Assert.That(result).IsEqualTo(1.5);
    }

    #endregion

    #region 2. Отрицательные углы

    [Test]
    public async Task Normalize_NegativePi_ReturnsPi()
    {
        // -π + 2π = π
        var result = Angle.Normalize(-Pi);
        await Assert.That(result).IsEqualTo(Pi).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_NegativeTwoPi_ReturnsZero()
    {
        // -2π + 2π = 0
        var result = Angle.Normalize(-TwoPi);
        await Assert.That(result).IsEqualTo(0).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_SmallNegative_ReturnsNormalized()
    {
        // -0.1 + 2π ≈ 6.18...
        var result = Angle.Normalize(-0.1);
        await Assert.That(result).IsEqualTo(TwoPi - 0.1).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_LargeNegative_MultipleRotations()
    {
        // -5π = -2*2π - π → должно стать π
        var result = Angle.Normalize(-5 * Pi);
        await Assert.That(result).IsEqualTo(Pi).Within(Epsilon);
    }

    #endregion

    #region 3. Углы больше 2π

    [Test]
    public async Task Normalize_ThreePi_ReturnsPi()
    {
        // 3π - 2π = π
        var result = Angle.Normalize(3 * Pi);
        await Assert.That(result).IsEqualTo(Pi).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_FourPi_ReturnsZero()
    {
        // 4π - 2*2π = 0
        var result = Angle.Normalize(4 * Pi);
        await Assert.That(result).IsEqualTo(0).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_TwoPiPlusSmall_ReturnsSmall()
    {
        // 2π + 0.5 - 2π = 0.5
        var result = Angle.Normalize(TwoPi + 0.5);
        await Assert.That(result).IsEqualTo(0.5).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_VeryLargeAngle_MultipleRotations()
    {
        // 100 * 2π + π/4 → должно стать π/4
        var result = Angle.Normalize(100 * TwoPi + Pi / 4);
        await Assert.That(result).IsEqualTo(Pi / 4).Within(Epsilon);
    }

    #endregion

    #region 4. Критические граничные случаи (Bug Hunting)

    [Test]
    public async Task Normalize_ExactlyTwoPi_DocumentationVsCode()
    {
        var result = Angle.Normalize(TwoPi);  
        await Assert.That(result).IsEqualTo(0).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_DoubleTwoPi_BoundaryCheck()
    {
        // 2 * (2π) = 4π. Цикл вычтет 2π два раза, результат должен быть 0.
        var result = Angle.Normalize(2 * TwoPi);
        await Assert.That(result).IsEqualTo(0).Within(Epsilon);
    }

    [Test]
    public async Task Normalize_FloatingPointPrecision_NearZero()
    {
        var result = Angle.Normalize(1e-15);
        await Assert.That(result).IsGreaterThanOrEqualTo(0);
        await Assert.That(result).IsLessThan(TwoPi);
    }

    [Test]
    public async Task Normalize_FloatingPointPrecision_NearTwoPi()
    {
        // Значение чуть меньше 2π должно остаться как есть
        var result = Angle.Normalize(TwoPi - 1e-10);
        await Assert.That(result).IsEqualTo(TwoPi - 1e-10).Within(Epsilon);
    }

    #endregion

    #region 5. Свойства нормализации (Property-based checks)

    [Test]
    [Arguments(0)]
    [Arguments(-1000)]
    [Arguments(1000)]
    [Arguments(-0.001)]
    [Arguments(0.001)]
    public async Task Normalize_AlwaysReturnsValidRange(double input)
    {
        var result = Angle.Normalize(input);
        
        // Главное свойство: результат ВСЕГДА в диапазоне [0, 2π]
        await Assert.That(result).IsGreaterThanOrEqualTo(0);
        await Assert.That(result).IsLessThanOrEqualTo(TwoPi);
    }

    [Test]
    public async Task Normalize_Idempotent_ApplyingTwiceGivesSameResult()
    {
        // Нормализация уже нормализованного угла не должна менять его
        var input = 123.456;
        var first = Angle.Normalize(input);
        var second = Angle.Normalize(first);
        
        await Assert.That(second).IsEqualTo(first);
    }

    #endregion
}