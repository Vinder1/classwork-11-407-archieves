namespace Algo.PriorityQueue;

public class MyPriorityQueue<T> : IPriorityQueue<T>
    where T : IComparable<T>
{
    private List<T> _nodes = [];
    
    //private Dictionary<T, int> _indexMap = new();

    private static int Parent(int i) => (i - 1) / 2;
    private static int LeftChild(int i) => 2*i + 1;
    
    public void Enqueue(T item)
    {
        _nodes.Add(item);
        
        var i = _nodes.Count - 1;
        while (i > 0 && _nodes[Parent(i)].CompareTo(_nodes[i]) > 0)
        {
            (_nodes[Parent(i)], _nodes[i]) = (_nodes[i], _nodes[Parent(i)]);
            i = Parent(i);
        }
        //_indexMap[item] = i;
    }

    public T Peek() => _nodes[0];
    public bool Empty => _nodes.Count == 0;

    public T Dequeue()
    {
        var min = _nodes[0];
        
        (_nodes[0], _nodes[^1]) = (_nodes[^1], _nodes[0]);
        
        //_indexMap.Remove(_nodes[^1]);
        //_indexMap[_nodes[0]] = 0;
        _nodes.RemoveAt(_nodes.Count - 1); //O(1)
        
        var i = 0;
        while ((i = LeftChild(i)) < _nodes.Count)
        {
            if (i + 1 < _nodes.Count && _nodes[i].CompareTo(_nodes[i + 1]) > 0)
            {
                i++;
            }

            if (_nodes[i].CompareTo(_nodes[Parent(i)]) < 0)
            {
                (_nodes[i], _nodes[Parent(i)]) = (_nodes[Parent(i)], _nodes[i]);
                //_indexMap[_nodes[i]] = i;
                //_indexMap[_nodes[Parent(i)]] = Parent(i);
            }
            else
            {
                break;
            }
        }

        return min;
    }


    private int Find(T item) => //_indexMap.GetValueOrDefault(item, -1);
        _nodes.IndexOf(item) < 0 ? -1 : _nodes.IndexOf(item);

    public bool Contains(T item) => _nodes.Contains(item); //_indexMap.ContainsKey(item);

    public void Remove(T item)
    {
        var index = Find(item);
        (_nodes[index], _nodes[^1]) = (_nodes[^1], _nodes[index]);
        
        //_indexMap.Remove(_nodes[^1]);
        _nodes.RemoveAt(_nodes.Count - 1); //O(1)
        
        if (index == _nodes.Count)
            return;
        //_indexMap[_nodes[index]] = index;
        
        var i = index;
        while (i > 0 && _nodes[i].CompareTo(_nodes[Parent(i)]) < 0)
        {
            (_nodes[i], _nodes[Parent(i)]) = (_nodes[Parent(i)], _nodes[i]);
            //_indexMap[_nodes[i]] = i;    
            i = Parent(i);
        }
        //_indexMap[_nodes[i]] = i;
        while ((i = LeftChild(i)) < _nodes.Count)
        {
            if (i + 1 < _nodes.Count && _nodes[i].CompareTo(_nodes[i + 1]) > 0)
            {
                i++;
            }

            if (_nodes[i].CompareTo(_nodes[Parent(i)]) < 0)
            {
                (_nodes[i], _nodes[Parent(i)]) = (_nodes[Parent(i)], _nodes[i]);
                //_indexMap[_nodes[i]] = i;
                //_indexMap[_nodes[Parent(i)]] = Parent(i);
            }
            else
            {
                break;
            }
        }
    }
}