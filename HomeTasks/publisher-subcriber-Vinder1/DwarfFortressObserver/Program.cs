namespace DwarfFortressObserver;

class Program
{
    static void Main(string[] args)
    {
        var militiaCommander = new MilitiaCommander();
        militiaCommander.Soldiers.Add(new Marksdwarf());
        militiaCommander.Soldiers.Add(new Marksdwarf());
        var bard = new BardDwarf();
        
        var scout = new FortressScout();
        scout.OnEnemyIncoming += militiaCommander.Update;
        scout.OnEnemyIncoming += bard.Update;
        
        Console.WriteLine("Kea-bird spotted");
        scout.SpotNewEnemy(new Enemy { Dangerous = false, Name = "Кеа" });
        
        Console.WriteLine("Forgotten beast spotted");
        scout.SpotNewEnemy(new Enemy { Dangerous = true, Name = "Acite The Great" });
    }
}