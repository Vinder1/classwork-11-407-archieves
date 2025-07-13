namespace Algo.PriorityQueue;

public interface IPriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    public void Enqueue(TElement item, TPriority priority);
    public TElement Peek();
    public TElement Dequeue();
    public bool Empty { get; }
}


