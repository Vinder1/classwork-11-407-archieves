using BenchmarkDotNet.Attributes;

namespace Algo.PriorityQueue;

public class PQTest
{
    private const int Number = 300_000;
    [Benchmark]
    public void BadPriorityQueueAdd()
    {
        var queue = new BadPriorityQueue<int>();
        for (var i = 0; i < Number; i++)
        {
            queue.Enqueue(Random.Shared.Next());
        }
    }
    
    [Benchmark]
    public void GoodPriorityQueueAdd()
    {
        var queue = new MyPriorityQueue<int>();
        for (var i = 0; i < Number; i++)
        {
            queue.Enqueue(Random.Shared.Next());
        }
    }
    
    [Benchmark]
    public void BadPriorityQueueAddAndRemove()
    {
        var queue = new BadPriorityQueue<int>();
        for (var i = 0; i < Number; i++)
        {
            queue.Enqueue(Random.Shared.Next());
        }

        while (!queue.Empty)
            queue.Dequeue();
    }
    
    [Benchmark]
    public void GoodPriorityQueueAddAndRemove()
    {
        var queue = new MyPriorityQueue<int>();
        for (var i = 0; i < Number; i++)
        {
            queue.Enqueue(Random.Shared.Next());
        }

        while (!queue.Empty)
            queue.Dequeue();
    }
    
    [Benchmark]
    public void SortedSetAdd()
    {
        var queue = new SortedSet<int>();
        for (var i = 0; i < Number; i++)
        {
            queue.Add(Random.Shared.Next());
        }
    }
    
    [Benchmark]
    public void SortedSetAddAndRemove()
    {
        var queue = new SortedSet<int>();
        for (var i = 0; i < Number; i++)
        {
            queue.Add(Random.Shared.Next());
        }
        
        while (queue.Any())
            queue.Remove(queue.Min);
    }
}