namespace DwarfFortressObserver;

public class BardDwarf : IObserver<Enemy>
{
    public void Update(Enemy value)
    {
        if (value.Name == "Кеа")
        {
            Console.WriteLine("[Bard] О, я знаю стихотворение!\n" +
                              " Кеа-птиц напал на дворфов! Ах, вот это номер!\n" +
                              " Кеа-птиц потешно жил, и потешно помер.");
            return;
        }

        if (value.Dangerous)
        {
            Console.WriteLine("[Bard] Ох, мы в дерьме! Я сваливаю");
        }
        else
        {
            Console.WriteLine("[Bard] Помнишь, брат, как давили напавшую мразь...");
        }
    }
}