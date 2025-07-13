namespace Algo.PriorityQueue;

public class BadPriorityQueue<T> : IPriorityQueue<T>
    where T : IComparable<T>
{
    private List<T> _data = [];
    
    public bool Empty => _data.Count == 0;
    public void Enqueue(T item) => _data.Add(item);
    public T Peek() => Minimum;
    public T Dequeue()
    {
        var min = Minimum;
        _data.Remove(min);
        return min;
    }
    private T Minimum => _data.Min()!;
}




