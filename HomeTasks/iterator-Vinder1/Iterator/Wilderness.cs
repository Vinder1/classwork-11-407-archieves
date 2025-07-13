using System.Collections;

namespace IteratorV2;

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
    public IEnumerator<Animal> GetEnumerator()
    {
        return new AnimalIterator(animals, CurrentPeriod);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}