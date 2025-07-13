using BenchmarkDotNet.Attributes;

namespace Algo.FenwickTree;

public class FenwickTreeTest
{
    private const int Length = 300_000;
    [Benchmark]
    public void SegmentTreeSum()
    {
        var array = new int[Length]; // Length - константа снаружи
        for (int i = 0; i < Length; i++)
        {
            array[i] = Random.Shared.Next() % 100;
        }
        
        var tree = new LazySegmentTree.LazySegmentTree(array);
        var index1 = Length / 4;
        var index2 = Length - 2;
        var sum = 0;
        for (int i = 0; i < 1000; i++)
            sum = tree.GetRangeSum(index1, index2);
    }
    
    [Benchmark]
    public void FenwickTreeSum()
    {
        var array = new int[Length];
        for (int i = 0; i < Length; i++)
        {
            array[i] = Random.Shared.Next();
        }
        
        var tree = new FenwickTree<int>(array);
        var index1 = Length / 4;
        var index2 = Length - 2;
        var sum = 0;
        for (int i = 0; i < 1000; i++)
            sum = tree.PrefixSum(index2) - tree.PrefixSum(index1-1);
    }
    
    [Benchmark]
    public void SimpleSum()
    {
        var array = new int[Length];
        for (int i = 0; i < Length; i++)
        {
            array[i] = Random.Shared.Next();
        }
        
        var index1 = Length / 4;
        var index2 = Length - 2;
        var sum = 0;
        for (var i = 0; i < 1000; i++)
        {
            for (var j = index1; j < index2; j++)
            {
                sum += array[j];
            }
        }
    }
}