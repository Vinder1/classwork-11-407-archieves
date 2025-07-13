namespace Reflextion;

public class BigCar : ICar
{
    public IEngine Engine { get; }

    public BigCar(string model)
    {
        Engine = null!;
        Console.WriteLine("Этот конструктор хочет, чтобы его никто не юзал!");    
    }
    
    public BigCar(IEngine engine)
    {
        Engine = engine;
        Console.WriteLine("Фура выпущена с производства!");
    }
}