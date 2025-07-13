using BenchmarkDotNet.Attributes;

namespace Algo.PriorityQueue;

public class PQSecondTest
{
    private const int Num = 300_000;
    [Benchmark]
    public void MyPriorityQueueAdd()
    {
        var queue = new MyPriorityQueue<int, int>();
        for (var i = 0; i < Num; i++)
        {
            queue.Enqueue(Random.Shared.Next(), Random.Shared.Next());
        }
    }
    
    [Benchmark]
    public void BuiltInPriorityQueueAdd()
    {
        var queue = new PriorityQueue<int, int>();
        for (var i = 0; i < Num; i++)
        {
            queue.Enqueue(Random.Shared.Next(), Random.Shared.Next());
        }
    }
    
    [Benchmark]
    public void MyPriorityQueueAddAndRemove()
    {
        var queue = new MyPriorityQueue<int, int>();
        for (var i = 0; i < Num; i++)
        {
            queue.Enqueue(Random.Shared.Next(), Random.Shared.Next());
        }
        
        while (!queue.Empty)
            queue.Dequeue();
    }
    
    [Benchmark]
    public void BuiltInPriorityQueueAddAndRemove()
    {
        var queue = new PriorityQueue<int, int>();
        for (var i = 0; i < Num; i++)
        {
            queue.Enqueue(Random.Shared.Next(), Random.Shared.Next());
        }
        
        while (queue.Count > 0)
            queue.Dequeue();
    }
}