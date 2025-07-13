namespace DwarfFortressObserver;

public class MilitiaCommander : IObserver<Enemy>
{
    public readonly List<Marksdwarf> Soldiers = [];
    
    public void Update(Enemy value)
    {
        Console.WriteLine($"[MilitiaCommander] Вперёд, собаки! К победе! Падшего ждёт слава!");
        if (value.Dangerous)
        {
            Console.WriteLine($"[MilitiaCommander] Напоминаю, если дворф не хочет сражаться - дворф будет оТшЛёПаН!");
        }

        foreach (var soldier in Soldiers)
        {
            soldier.Update(value);
        }
    }
}