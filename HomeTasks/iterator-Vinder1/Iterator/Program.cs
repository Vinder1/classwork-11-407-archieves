namespace IteratorV2;

class Program
{
    static void Main(string[] args)
    {
        var animalCollection = new Wilderness([
            new Animal("Заяц", ActivityPeriod.Day),
            new Animal("Бабочка", ActivityPeriod.Day),
            new Animal("Кабан", ActivityPeriod.Night),
            new Animal("Обезьяна", ActivityPeriod.Day),
            new Animal("Ящерица", ActivityPeriod.Day),
            new Animal("Волк", ActivityPeriod.Night),
            new Animal("Сова", ActivityPeriod.Night),
        ]);
        
        Console.WriteLine("Обход всех животных...");
        foreach (var animal in animalCollection)
        {
            Console.WriteLine($"{animal.Name} ");
        }
        
        Console.WriteLine("Наступает утро, просыпаются дневные животные...");
        animalCollection.CurrentPeriod = ActivityPeriod.Day;
        foreach (var animal in animalCollection)
        {
            Console.WriteLine($"{animal.Name} ");
        }
        
        Console.WriteLine("Наступает ночь, просыпаются ночные твари...");
        animalCollection.CurrentPeriod = ActivityPeriod.Night;
        foreach (var animal in animalCollection)
        {
            Console.WriteLine($"{animal.Name} ");
        }
    }
}