namespace DwarfFortressObserver;

public class Marksdwarf : IObserver<Enemy>
{
    public void Update(Enemy value)
    {
        Console.WriteLine($"[Marksdwarf] Готовится к бою с {value.Name}...");
    }
}