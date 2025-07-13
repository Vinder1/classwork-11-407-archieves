using System.Reflection;
using System.Text.Json;

namespace StreamLinq;

class Program
{
    static void Main(string[] args)
    {
        var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!}/Wilderness";
        
        /*var animalCollection = new Wilderness([
            new Animal("Заяц", ActivityPeriod.Day),
            new Animal("Бабочка", ActivityPeriod.Day),
            new Animal("Кабан", ActivityPeriod.Night),
            new Animal("Обезьяна", ActivityPeriod.Day),
            new Animal("Ящерица", ActivityPeriod.Day),
            new Animal("Волк", ActivityPeriod.Night),
            new Animal("Сова", ActivityPeriod.Night),
        ]);
        animalCollection.SaveToJson(path+".json");*/

        var animalCollection = new Wilderness([]);
        animalCollection.LoadFromJson(path+".json");
        animalCollection.SaveToYaml(path + ".yaml");
        
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
        
        
        //Linq tests
        Console.WriteLine("\nВсе ли животные ночные?");
        Console.WriteLine(animalCollection.All(ActivityPeriod.Night));

        Console.WriteLine("Группировка по периодам");
        foreach (var group in animalCollection.GroupByPeriod())
        {
            Console.WriteLine($"{group.Key} {string.Join("-", group.Select(animal => animal.Name))}");
        }

        Console.WriteLine($"В коллекции есть сова? {animalCollection.Contains("Сова")}");
        Console.WriteLine($"В коллекции есть лев? {animalCollection.Contains("Лев")}");
        
        Console.WriteLine($"А теперь, обход всех животных: сначала дневные, потом ночные");
        foreach (var animal in animalCollection.OrderByNameAndPeriod())
        {
            Console.WriteLine(animal.Name);
        }
    }
}