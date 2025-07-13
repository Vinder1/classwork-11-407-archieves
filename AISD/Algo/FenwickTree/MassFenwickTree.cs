using System.Numerics;

namespace Algo.FenwickTree;

public class MassFenwickTree : IMassFenwickTree<int>
{
    private int[] sum;
    private int[] updateInfo;

    public MassFenwickTree(int n)
    {
        sum = new int[n+1];
        updateInfo = new int[n+1];
    }

    public MassFenwickTree(int[] array) : this(array.Length)
    {
        for(var i = 1; i < sum.Length; i++)
        {
            sum[i] += array[i-1];
            if(i + (i&-i) < sum.Length)
                sum[i + (i&-i)] += sum[i];
        }
    }
    
    public int PrefixSum(int index)
    {
        index++;
        var result = 0;
        for (var i = index; i > 0; i -= i &- i)
        {
            result += sum[i];
            result += updateInfo[i] * (i & -i);
        }
        for (var i = index + (index&-index); i < sum.Length; i += i & -i)
        {
            result += updateInfo[i] * (index - (i - (i & -i)));
        }

        return result;
    }

    public void PrefixAdd(int index, int value)
    {
        index++;
        
        for (var i = index; i > 0; i -= i &- i)
        {
            updateInfo[i] += value;
        }
        for (var i = index; i < sum.Length; i += i & -i)
        {
            sum[i] += value * (index - (i - (i & -i)));
        }
    }
}