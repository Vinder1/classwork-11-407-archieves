using System.Collections;

namespace StreamLinq;

public class AnimalIterator : IEnumerator<Animal>
{
    public ActivityPeriod AnimalsType { get; }
    
    private int _current = -1;
    private Animal[]? _items;

    public AnimalIterator(Animal[] items, ActivityPeriod type)
    {
        _items = items;
        AnimalsType = type;
    }
    
    public bool MoveNext()
    {
        if (_current == _items!.Length-1)
        {
            return false;
        }

        _current++;
        if (AnimalsType == ActivityPeriod.All)
        {
            return true;
        }
        
        while (_current < _items!.Length - 1 && _items[_current].ActivityPeriod != AnimalsType)
        {
            _current++;
        }

        if (_current == _items!.Length - 1)
        {
            return false;
        }

        return true;
    }

    public void Reset()
    {
        _current = 0;
    }

    public Animal Current => _items![_current];

    object? IEnumerator.Current => Current;

    public void Dispose()
    {
        _items = null;
    }
}