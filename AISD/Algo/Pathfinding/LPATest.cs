using AStar.Nodes;
using BenchmarkDotNet.Attributes;

namespace Algo.Pathfinding;

public class LPATest
{
    [Params(500, 700, 1000)] public int N;
    //public int N = 100;
    //[Params(100)]
    public int Iterations = 10;

    [Benchmark]
    public void LpaStarTest()
    {
        var pathfinder = new LpaStar(N, N, (0,0), (N-1, N-1));
        pathfinder.Run();
        for (var _ = 0; _ < Iterations; _++)
        {
            pathfinder.Update([
                (Random.Shared.Next(0, N - 1), Random.Shared.Next(0, N - 1),Random.Shared.Next(0, 10)),
                (Random.Shared.Next(0, N - 1), Random.Shared.Next(0, N - 1),Random.Shared.Next(0, 10)),
                (Random.Shared.Next(0, N - 1), Random.Shared.Next(0, N - 1),Random.Shared.Next(0, 10)),
                (Random.Shared.Next(0, N - 1), Random.Shared.Next(0, N - 1),Random.Shared.Next(0, 10)),
                (Random.Shared.Next(0, N - 1), Random.Shared.Next(0, N - 1),Random.Shared.Next(0, 10))
            ]);
        }
        // var grid = new GridNode[N, N];
        // for (var i = 0; i < N; i++)
        // {
        //     for (var j = 0; j < N; j++)
        //     {
        //         grid[i, j] = new GridNode(j, i);
        //         grid[i, j].Weight = Random.Shared.Next(1, 10);
        //     }
        // }
        // //
        // var pathfinder = new LpaStar(N, N, (0,0), (N-1, N-1));
        // pathfinder.Run();
    }

    [Benchmark]
    public void AStarTest()
    {
        var grid = new GridNode[N, N];
        for (var i = 0; i < N; i++)
        {
            for (var j = 0; j < N; j++)
            {
                grid[i, j] = new GridNode(j, i);
            }
        }
        
        AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0, 0], grid[N - 1, N - 1]);
        for (var _ = 0; _ < Iterations; _++)
        {
            grid = new GridNode[N, N];
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    grid[i, j] = new GridNode(i, j);
                    grid[i, j].Weight = Random.Shared.Next(1, 10);
                }
            }
            grid[1, 1].Weight = Random.Shared.Next(1, 10);
            grid[2, 2].Weight = Random.Shared.Next(1, 10);
            grid[1, 2].Weight = Random.Shared.Next(1, 10);
            grid[2, 1].Weight = Random.Shared.Next(1, 10);
            grid[1, 3].Weight = Random.Shared.Next(1, 10);
            var path = AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0, 0], grid[N - 1, N - 1]);
            if (path == null)
                throw new Exception("Path not found");
        }
        
        // var grid = new GridNode[N, N];
        // for (var i = 0; i < N; i++)
        // {
        //     for (var j = 0; j < N; j++)
        //     {
        //         grid[i, j] = new GridNode(i, j);
        //         grid[i, j].Weight = 1;
        //         //grid[i, j].Weight = Random.Shared.Next(1, 10);
        //     }
        // }
        //
        // AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0, 0], grid[N - 1, N - 1]);
    }
    //
    // [Benchmark]
    // public void EdinayaRussia()
    // {
    //     var N = 100;
    //     N++;
    // }
}