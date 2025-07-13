namespace DwarfFortressObserver;

public class FortressScout : IDelegateObservable<Enemy>
{
    public event Action<Enemy> OnEnemyIncoming =
        enemy => { Console.WriteLine($"[Scout] Враг на границе! Это {enemy.Name}!"); };

    public void NotifyObservers(Enemy enemy)
    {
        OnEnemyIncoming.Invoke(enemy);
    }

    public void SpotNewEnemy(Enemy enemy) => NotifyObservers(enemy);
}