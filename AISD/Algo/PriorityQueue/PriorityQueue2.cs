namespace Algo.PriorityQueue;

public class MyPriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>
    where TPriority : IComparable<TPriority>
{
    private record Node (TElement Element, TPriority Priority);
    
    private List<Node> _nodes = [];

    private static int Parent(int i) => (i - 1) / 2;
    private static int LeftChild(int i) => 2*i + 1;
    
    public void Enqueue(TElement item, TPriority priority)
    {
        _nodes.Add(new Node(item, priority));
        var i = _nodes.Count - 1;
        while (i > 0 && _nodes[Parent(i)].Priority.CompareTo(_nodes[i].Priority) > 0)
        {
            (_nodes[Parent(i)], _nodes[i]) = (_nodes[i], _nodes[Parent(i)]);
            i = Parent(i);
        }
    }

    public TElement Peek() => _nodes[0].Element;
    public bool Empty => _nodes.Count == 0;

    public TElement Dequeue()
    {
        var min = _nodes[0];
        
        (_nodes[0], _nodes[^1]) = (_nodes[^1], _nodes[0]);
        _nodes.RemoveAt(_nodes.Count - 1); //O(1)
        var i = 0;
        while ((i = LeftChild(i)) < _nodes.Count)
        {
            if (i + 1 < _nodes.Count && _nodes[i].Priority.CompareTo(_nodes[i + 1].Priority) > 0)
            {
                i++;
            }

            if (_nodes[i].Priority.CompareTo(_nodes[Parent(i)].Priority) < 0)
            {
                (_nodes[i], _nodes[Parent(i)]) = (_nodes[Parent(i)], _nodes[i]);
            }
            else
            {
                break;
            }
        }

        return min.Element;
    }
}