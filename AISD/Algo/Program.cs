using System.Diagnostics;
using Algo.FenwickTree;
using Algo.Pathfinding;
using Algo.PriorityQueue;
using AStar.Nodes;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algo;

class Program
{
    static void Main(string[] args)
    {
        //IMassFenwickTree<int> a = new MassFenwickTree([0, 0, 0, 0, 0, 0, 0, 0, 0]);
        //a.PrefixAdd(6, 1);
        // Console.WriteLine(a.PrefixSum(0));
        // Console.WriteLine(a.PrefixSum(1));
        // Console.WriteLine(a.PrefixSum(2));
        // Console.WriteLine(a.PrefixSum(3));
        //Console.WriteLine(a.PrefixSum(4));
        //Console.WriteLine(a.PrefixSum(5));
        // a.PrefixAdd(2, 1);
        // Console.WriteLine(a.PrefixSum(4));
        // MyPriorityQueue<string, int> queue = new();
        // queue.Enqueue("A", 42);
        // queue.Enqueue("B", 15);
        // queue.Enqueue("C", 2);
        //
        // while (!queue.Empty)
        // {
        //     Console.WriteLine(queue.Dequeue());
        // } // C B A

        //FenwickTreeTest.Length = int.Parse(args[0]);
        // var t = new FenwickTreeTest();
        // t.FenwickTreeSum();
        //Console.WriteLine(FenwickTreeTest.Length);

        //BenchmarkRunner.Run<MassFenwickTreeTest>();
        // var queue = new MyPriorityQueue<int>();
        // queue.Enqueue(1);
        // queue.Enqueue(2);
        // queue.Enqueue(3);
        // queue.Enqueue(4);
        // queue.Enqueue(5);
        // queue.Enqueue(6);
        // queue.Enqueue(7);
        //
        // queue.Remove(3);
        // while (!queue.Empty)
        // {
        //     Console.WriteLine(queue.Dequeue());
        // }
        //
        // var n = 100;
        // var stopwatch = Stopwatch.StartNew();
        // var pathfinder = new LpaStar(n, n, (0, 0), (n-1, n-1));
        // pathfinder.Run();
        // pathfinder.Update([
        //     (1, 0, 100),
        //     (1, 1, 2),
        //     (2, 1, 2),
        //     (3, 1, 2),
        // ]);
        
        // pathfinder = new LpaStar(n, n, (0, 0), (n-1, n-1));
        // pathfinder.Run();
        // stopwatch.Stop();
        // Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
        //
        // stopwatch.Restart();
        // pathfinder = new LpaStar(n, n, (0, 0), (n-1, n-1));
        // pathfinder.Run();
        // stopwatch.Stop();
        // Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
        //

        // var pathfinder = new LpaStar(10, 10, (0, 0), (9, 9));
        // pathfinder.Run();
        // pathfinder.Print();
        // pathfinder.Update([
        //     (1, 0, 100),
        //     (1, 1, 2),
        //     (2, 1, 2),
        //     (3, 1, 2),
        // ]);
        // pathfinder.Print();
        // pathfinder.Update([
        //     (6, 2, 2),
        //     (6, 3, 2),
        //     (6, 4, 2),
        // ]);
        // pathfinder.Print();
        // pathfinder.Update([
        //     (5, 4, 2),
        // ]);
        // // pathfinder.Print();
        // Stopwatch stopwatch;
        // GridNode[,] grid;
        //
        // grid = new GridNode[n, n];
        // for (var i = 0; i < n; i++)
        // {
        //     for (var j = 0; j < n; j++)
        //     {
        //         grid[i,j] = new GridNode(i, j);
        //         grid[i, j].Weight = 1;
        //     }
        // }
        //
        // stopwatch = Stopwatch.StartNew();
        // AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0,0], grid[n-1,n-1]);
        // stopwatch.Stop();
        // Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
        //
        // grid = new GridNode[n, n];
        // for (var i = 0; i < n; i++)
        // {
        //     for (var j = 0; j < n; j++)
        //     {
        //         grid[i,j] = new GridNode(i, j);
        //         grid[i, j].Weight = 1;
        //     }
        // }
        //
        // stopwatch = Stopwatch.StartNew();
        // AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0,0], grid[n-1,n-1]);
        // stopwatch.Stop();
        //Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
        //
        // stopWatch.Stop();
        // Console.WriteLine(stopWatch.Elapsed.TotalMilliseconds);
        // int n = 100;
        // var grid = new GridNode[n, n];
        // for (var i = 0; i < n; i++)
        // {
        //     for (var j = 0; j < n; j++)
        //     {
        //         grid[i, j] = new GridNode(i, j);
        //         grid[i, j].Weight = Random.Shared.Next(1, 10);
        //         //grid[i, j].Walkable = true;
        //     }
        // }
        //
        // var pathfinder = new LpaStar(grid, (0, 0), (n - 1, n - 1));
        // pathfinder.Run();
        // var path = pathfinder.GetPath((0,0), (n - 1, n - 1));
        // var path2 = AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0,0], grid[n-1,n-1]);
        // var sum1 = path.Sum(x => x.Weight);
        // var sum2 = path2.Sum(x => x.Weight);
        // Console.WriteLine($"LPA: {sum1} A*: {sum2}");
        BenchmarkRunner.Run<LPATest>();
        //new LPATest().LpaStarTest();
        
        // Stopwatch stopwatch = Stopwatch.StartNew();
        //
        // var N = 100;
        // var Iterations = 20;
        //
        // var pathfinder = new LpaStar(N, N, (0,0), (N-1, N-1));
        // pathfinder.Run();
        // for (var _ = 0; _ < Iterations; _++)
        // {
        //     pathfinder.Update([(Random.Shared.Next(0, N - 1), Random.Shared.Next(0, N - 1),Random.Shared.Next(0, 10))]);
        // }
        //
        // stopwatch.Stop();
        // Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
        //
        // stopwatch = Stopwatch.StartNew();
        //
        // var grid = new GridNode[N, N];
        // for (var i = 0; i < N; i++)
        // {
        //     for (var j = 0; j < N; j++)
        //     {
        //         grid[i, j] = new GridNode(j, i);
        //     }
        // }
        //
        // AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0, 0], grid[N - 1, N - 1]);
        // for (var _ = 0; _ < Iterations; _++)
        // {
        //     grid = new GridNode[N, N];
        //     for (var i = 0; i < N; i++)
        //     {
        //         for (var j = 0; j < N; j++)
        //         {
        //             grid[i, j] = new GridNode(i, j);
        //             grid[i, j].Weight = Random.Shared.Next(1, 10);
        //         }
        //     }
        //     grid[1, 1].Weight = Random.Shared.Next(1, 10);
        //     var path = AStar.Algorithms.GridAStarWithWeights.FindPath(grid, grid[0, 0], grid[N - 1, N - 1]);
        //     if (path == null)
        //         throw new Exception("Path not found");
        // }
        //
        // stopwatch.Stop();
        // Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
    }
}