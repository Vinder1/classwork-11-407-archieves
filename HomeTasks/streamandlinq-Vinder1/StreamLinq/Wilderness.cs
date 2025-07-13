using System.Collections;
using System.Reflection;
using System.Text.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace StreamLinq;

/// <summary>
/// Wild zone where animals live
/// </summary>
public class Wilderness : IEnumerable<Animal>
{
    public ActivityPeriod CurrentPeriod { get; set; } = ActivityPeriod.All;
    private Animal[] animals;
    
    public Wilderness(Animal[] animals)
    {
        this.animals = animals;
    }
    
    //Linq
    public IEnumerable<IGrouping<ActivityPeriod, Animal>> GroupByPeriod()
        => animals.GroupBy(animal => animal.ActivityPeriod);
    
    public bool All(ActivityPeriod period)
        => animals.All(animal => animal.ActivityPeriod == period);
    
    public bool Contains(string name)
        => animals.Any(animal => animal.Name == name);
    
    public IEnumerable<Animal> OrderByNameAndPeriod()
        => animals.OrderBy(animal => animal.ActivityPeriod)
            .ThenBy(animal => animal.Name);
    
    //(De)Serialization
    
    private record WildernessContainer (ActivityPeriod Period, Animal[] Animals);
    
    public void SaveToJson(string path)
    {
        using var writer = new FileStream(path, FileMode.Truncate);
        JsonSerializer.Serialize(writer, new WildernessContainer(CurrentPeriod, animals));
    }
    
    public void LoadFromJson(string path)
    {
        using var writer = new FileStream(path, FileMode.OpenOrCreate);
        (CurrentPeriod, animals) = JsonSerializer.Deserialize<WildernessContainer>(writer)!;
    }
    
    public void SaveToYaml(string path)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        using var writer = new StreamWriter(path);
        serializer.Serialize(writer, new WildernessContainer(CurrentPeriod, animals));
    }

    //IEnumerable implementation
    public IEnumerator<Animal> GetEnumerator()
    {
        return new AnimalIterator(animals, CurrentPeriod);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}