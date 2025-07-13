using System.Numerics;

namespace Algo.FenwickTree;

public interface IFenwickTree<T>
    where T : INumber<T>
{
    public T PrefixSum(int index);
    public void Add(int index, T value);
}