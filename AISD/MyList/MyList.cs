using System.Collections;

namespace MyList;

public class MyList<T> : IMyList<T>
{
    public T[] _array = [default(T)];
    private int _length = 0;

    private int _count;

    public T this[int i]
    {
        get => _array[i];
        set => _array[i] = value;
    }

    public IEnumerator<T> GetEnumerator()
    {
        _count = 0;
        while (_count < _length)
            yield return _array[_count++];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }

    public bool Contains(T item)
    {
        for (var i = 0; i < _length; i++)
        {
            if (_array[i]!.Equals(item))
            {
                return true;
            }
        }

        return false;
    }

    public void Add(T item)
    {
        if (_array.Length == _length)
        {
            Array.Resize(ref _array, _length * 2);
        }

        _array[_length++] = item;
    }

    public void Remove(T item)
    {
        for (var i = 0; i < _length; i++)
        {
            if (!_array[i]!.Equals(item))
            {
                continue;
            }
            
            //_array[i] = default(T);
            for (var j = i; j < _length - 1; j++)
            {
                (_array[j], _array[j + 1]) = (_array[j + 1], _array[j]);
            }
            _length--;
            break;
        }
    }

    public void Clear()
    {
        _array = [];
    }

    public int IndexOf(T item)
    {
        for (var i = 0; i < _length; i++)
        {
            if (_array[i]!.Equals(item))
            {
                return i;
            }
        }

        return -1;
    }

    public void Reverse()
    {
        throw new NotImplementedException();
    }
}