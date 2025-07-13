using System.Numerics;

namespace Algo.FenwickTree;

public class FenwickTree<T> : IFenwickTree<T> 
    where T : INumber<T>
{
    private T[] sum;

    public FenwickTree(int n)
    {
        sum = new T[n+1];
    }

    public FenwickTree(T[] array) : this(array.Length)
    {
        for(var i = 1; i < sum.Length; i++)
        {
            sum[i] += array[i-1];
            if(i + (i&-i) < sum.Length)
                sum[i + (i&-i)] += sum[i];
        }
    }
    
    public T PrefixSum(int index)
    {
        index++;
        var result = default(T)!;
        for (var i = index; i > 0; i -= i &- i)
        {
            result += sum[i];
        }

        return result;
    }

    public void Add(int index, T value)
    {
        index++;

        for (var i = index; i < sum.Length; i += i & -i)
        {
            sum[i] += value;
        }
    }
}