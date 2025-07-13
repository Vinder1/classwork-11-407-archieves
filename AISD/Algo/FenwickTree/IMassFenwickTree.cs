using System.Numerics;

namespace Algo.FenwickTree;

public interface IMassFenwickTree<T>
    where T : INumber<T>
{
    public T PrefixSum(int index);
    public void PrefixAdd(int index, T value);
}