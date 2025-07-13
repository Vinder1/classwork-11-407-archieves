namespace ParallelImageSmoothing;

public class ParallelSmoother(bool[,] picture, int ThreadNum, int D, int Iterations)
{
    private bool[,] Picture => picture;
    private int[,] _neighborCount = null!;
    private int Height => Picture.GetLength(0);
    private int Width => Picture.GetLength(1);
    
    private bool _changed;
    private readonly object _changedLock = new object();

    public void Smooth()
    {
        _neighborCount = new int[Height, Width];

        using var counter = new CountdownEvent(ThreadNum);
        for (var _ = 0; _ < Iterations; _++)
        {
            counter.Reset();
            Parallel.For(0, ThreadNum, i => ClearNeighborCount(i, counter));
            counter.Wait();

            counter.Reset();
            Parallel.For(0, ThreadNum, i => HandleShareOnSameLineThread(i, counter));
            counter.Wait();
            
            counter.Reset();
            Parallel.For(0, ThreadNum, i => HandleShareThread(i, counter));
            counter.Wait();

            _changed = false;
            counter.Reset();
            Parallel.For(0, ThreadNum, i => HandleCalculateThread(i, counter));
            counter.Wait();

            if (!_changed)
                return;
        }
    }

    private void ClearNeighborCount(int threadId, CountdownEvent counter)
    {
        for (var line = threadId; line < Height; line += ThreadNum)
        {
            for (var j = 0; j < Width; j++)
            {
                _neighborCount[line, j] = 0;
            }
        }
        counter.Signal();
    }

    private void HandleShareOnSameLineThread(int threadId, CountdownEvent counter)
    {
        for (var line = threadId; line < Height; line += ThreadNum)
        {
            for (var i = 1; i < Width - 1; i++)
            {
                if (!Picture[line, i]) 
                    continue;
                _neighborCount[line, i + 1]++;
                _neighborCount[line, i - 1]++;
            }
            if (Picture[line, 0])
                _neighborCount[line, 1]++;
            if (Picture[line, Width - 1])
                _neighborCount[line, Width - 2]++;
        }
        counter.Signal();
    }

    private void HandleShareThread(int threadId, CountdownEvent counter)
    {
        for (var line = threadId; line < Height; line += ThreadNum)
        {
            //Upper line
            if (line > 0)
            {
                //Up
                for (var i = 0; i < Width; i++)
                {
                    if (Picture[line, i])
                    {
                        Interlocked.Increment(ref _neighborCount[line - 1, i]);
                    }
                }

                //Right
                for (var i = 0; i < Width - 1; i++)
                {
                    if (Picture[line, i])
                    {
                        Interlocked.Increment(ref _neighborCount[line - 1, i + 1]);
                    }
                }

                //Left
                for (var i = 1; i < Width; i++)
                {
                    if (Picture[line, i])
                    {
                        Interlocked.Increment(ref _neighborCount[line - 1, i - 1]);
                    }
                }
            }

            //Down line
            if (line < Height - 1)
            {
                //Down
                for (var i = 0; i < Width; i++)
                {
                    if (Picture[line, i])
                    {
                        Interlocked.Increment(ref _neighborCount[line + 1, i]);
                    }
                }

                //Right
                for (var i = 0; i < Width - 1; i++)
                {
                    if (Picture[line, i])
                    {
                        Interlocked.Increment(ref _neighborCount[line + 1, i + 1]);
                    }
                }

                //Left
                for (var i = 1; i < Width; i++)
                {
                    if (Picture[line, i])
                    {
                        Interlocked.Increment(ref _neighborCount[line + 1, i - 1]);
                    }
                }
            }
        }

        counter.Signal();
    }

    private void HandleCalculateThread(int threadId, CountdownEvent counter)
    {
        for (var line = threadId; line < Height; line += ThreadNum)
        {
            for (var i = 0; i < Width; i++)
            {
                var sum = _neighborCount[line, i];
                if (Picture[line, i] && 8 - sum >= D || !Picture[line, i] && sum >= D)
                {
                    Picture[line, i] = !Picture[line, i];
                    
                    _changed = true; // no need in lock actually
                    // Also, without lock, there is a 2x performance boost on big pictures
                }
            }
        }

        counter.Signal();
    }
    
    public static int SumOfNeighborSquares(bool[,] picture, int i, int j)
    {
        var height = picture.GetLength(0);
        var width = picture.GetLength(1);
        var sum = 0;
        for (var i1 = i - 1; i1 <= i + 1; i1++)
        {
            for (var j1 = j - 1; j1 <= j + 1; j1++)
            {
                if (i1 == i && j1 == j
                    || i1 < 0 || i1 > height - 1
                    || j1 < 0 || j1 > width - 1)
                {
                    continue;
                }

                sum += picture[i1, j1] ? 1 : 0;
            }
        }

        return sum;
    }
}