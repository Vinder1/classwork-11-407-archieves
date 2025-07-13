namespace Algo.PriorityQueue;

public interface IPriorityQueue<T> where T : IComparable<T>
{
    public void Enqueue(T item);
    public T Peek();
    public T Dequeue();
    public bool Empty { get; }
}

