namespace Algo.LazySegmentTree;

public class LazySegmentTree
{
    private readonly int[] _tree;
    private readonly int[] _lazy;
    private readonly int _arrayLength;
    
    public LazySegmentTree(int[] arr)
    {
        _arrayLength = arr.Length;
        var n = (int)Math.Ceiling(Math.Log(_arrayLength) / Math.Log(2));
        var threeLength = 2 * (int)Math.Pow(2, n) - 1;

        _tree = new int[threeLength];
        _lazy = new int[threeLength];

        BuildSegmentTreePart(arr, 0, _arrayLength - 1, 1);
    }
    
    public int GetRangeSum(int queryStart, int queryEnd)
    {
        return ProcessSumInRange(0, _arrayLength - 1, queryStart, queryEnd, 1);
    }

    public void UpdateRange(int queryStart, int queryEnd, int difference)
    {
        ProcessRangeUpdate(1, 0, _arrayLength - 1, queryStart, queryEnd, difference);
    }

    private void ProcessRangeUpdate(int nodeIndex, int currentStart, int currentEnd, int queryStart,
        int queryEnd, int difference)
    {
        PushUpdatesFromLazyNode(nodeIndex, currentStart, currentEnd);

        if (currentStart > currentEnd || currentStart > queryEnd || currentEnd < queryStart)
            return;

        if (currentStart >= queryStart && currentEnd <= queryEnd)
        {
            _tree[nodeIndex] += (currentEnd - currentStart + 1) * difference;

            if (currentStart != currentEnd)
            {
                _lazy[nodeIndex * 2] += difference;
                _lazy[nodeIndex * 2 + 1] += difference;
            }
        }
        else
        {
            var mid = (currentStart + currentEnd) / 2;

            ProcessRangeUpdate(nodeIndex * 2, currentStart, mid, queryStart, queryEnd, difference);
            ProcessRangeUpdate(nodeIndex * 2 + 1, mid + 1, currentEnd, queryStart, queryEnd, difference);

            _tree[nodeIndex] = _tree[nodeIndex * 2] + _tree[nodeIndex * 2 + 1];
        }
    }

    private void PushUpdatesFromLazyNode(int nodeIndex, int currentStart, int currentEnd)
    {
        if (_lazy[nodeIndex] == 0)
        {
            return;
        }

        _tree[nodeIndex] += (currentEnd - currentStart + 1) * _lazy[nodeIndex];

        if (currentStart != currentEnd)
        {
            _lazy[nodeIndex * 2] += _lazy[nodeIndex];
            _lazy[nodeIndex * 2 + 1] += _lazy[nodeIndex];
        }

        _lazy[nodeIndex] = 0;
    }

    private int ProcessSumInRange(int currentStart, int currentEnd, int queryStart, int queryEnd, int nodeIndex)
    {
        PushUpdatesFromLazyNode(nodeIndex, currentStart, currentEnd);

        if (currentStart > currentEnd || currentStart > queryEnd || currentEnd < queryStart)
            return 0;

        if (currentStart >= queryStart && currentEnd <= queryEnd)
        {
            return _tree[nodeIndex];
        }

        var mid = (currentStart + currentEnd) / 2;

        return ProcessSumInRange(currentStart, mid, queryStart, queryEnd, 2 * nodeIndex) +
               ProcessSumInRange(mid + 1, currentEnd, queryStart, queryEnd, 2 * nodeIndex + 1);
    }

    private void BuildSegmentTreePart(int[] arr, int currentStart, int currentEnd, int nodeIndex)
    {
        if (currentStart == currentEnd)
        {
            _tree[nodeIndex] = arr[currentStart];
        }
        else
        {
            var mid = (currentStart + currentEnd) / 2;
            BuildSegmentTreePart(arr, currentStart, mid, nodeIndex * 2);
            BuildSegmentTreePart(arr, mid + 1, currentEnd, nodeIndex * 2 + 1);
            _tree[nodeIndex] = _tree[nodeIndex * 2] + _tree[nodeIndex * 2 + 1];
        }
    }
}