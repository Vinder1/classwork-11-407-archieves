using Algo.PriorityQueue;
using AStar.Nodes;

namespace Algo.Pathfinding;

using System;
using System.Collections.Generic;

// Узел графа
public class Node : IComparable<Node>
{
    public int X { get; }
    public int Y { get; }
    public int Weight { get; set; }
    public Node Goal { get; }

    public double G { get; set; } // Стоимость от начального узла до текущего
    public double Rhs { get; set; } // Односторонняя оценка

    public Node(int y, int x, int weight, Node? goal = null)
    {
        X = x;
        Y = y;
        Weight = weight;
        Goal = goal ?? this;
        G = double.MaxValue;
        Rhs = double.MaxValue;
    }

    public int CompareTo(Node? other)
    {
        if (other is null)
            return 1;

        var key1 = Math.Min(G, Rhs) + LpaStar.Heuristic(this, Goal);
        var key2 = Math.Min(other.G, other.Rhs) + LpaStar.Heuristic(other, Goal);

        if (!Equals(key1, key2))
            return key1.CompareTo(key2);

        var key1_2 = Math.Min(G, Rhs);
        var key2_2 = Math.Min(other.G, other.Rhs);
        return key1_2.CompareTo(key2_2);
    }
}

public class LpaStar
{
    private int Rows { get; }
    private int Cols { get; }

    private readonly Node[,] _nodes;
    private readonly MyPriorityQueue<Node> _queue = new();
    private Node _start = null!;
    private Node _goal = null!;

    private const double E = 1.0e-10;

    public LpaStar(int rows, int cols, (int y, int x) start, (int y, int x) goal)
    {
        Rows = rows;
        Cols = cols;
        _nodes = new Node[Rows, Cols];
        InitializeGraph(start, goal);
    }

    public LpaStar(GridNode[,] grid, (int y, int x) start, (int y, int x) goal)
    {
        Rows = grid.GetLength(0);
        Cols = grid.GetLength(1);
        _nodes = new Node[Rows, Cols];
        InitializeGraph(start, goal, grid);
    }

    public void Run()
    {
        ComputeShortestPath();
    }

    public void Print()
    {
        // Console.WriteLine("Путь:");
        // foreach (var node in GetPath(_start, _goal))
        // {
        //     Console.WriteLine($"({node.X}, {node.Y})");
        // }
        var matrix = new char[Rows, Cols];
        for (var y = 0; y < Rows; y++)
        {
            for (var x = 0; x < Cols; x++)
            {
                matrix[y, x] = _nodes[y, x].Weight == 1 ? ' ' : '\u25a0';
            }
        }

        foreach (var node in GetPath(_start, _goal))
        {
            matrix[node.Y, node.X] = '*';
        }

        for (var y = Rows - 1; y >= 0; y--)
        {
            for (var x = 0; x < Cols; x++)
            {
                Console.Write(matrix[y, x]);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public void Update((int y, int x, int weight)[] updatedNodes)
    {
        foreach (var node in updatedNodes)
        {
            _nodes[node.y, node.x].Weight = node.weight;
            UpdateVertex(_nodes[node.y, node.x]);
        }

        ComputeShortestPath();
    }

    private void InitializeGraph((int y, int x) start, (int y, int x) goal, GridNode[,]? grid = null)
    {
        _goal = _nodes[goal.y, goal.x]
            = new Node(goal.y, goal.x, 1);
        if (grid == null)
        {
            for (var x = 0; x < Cols; x++)
            {
                for (var y = 0; y < Rows; y++)
                {
                    if (x == goal.x && y == goal.y)
                        continue;
                    _nodes[y, x] = new Node(y, x, 1, _goal);
                }
            }
        }
        else
        {
            _goal.Weight = (int)grid[goal.y, goal.x].Weight;
            for (var x = 0; x < Cols; x++)
            {
                for (var y = 0; y < Rows; y++)
                {
                    if (x == goal.x && y == goal.y)
                        continue;
                    _nodes[y, x] = new Node(y, x, (int)grid[y, x].Weight, _goal);
                }
            }
        }

        _start = _nodes[start.y, start.x];

        _start.Rhs = 0;
        _queue.Enqueue(_start);
    }

    public static double Heuristic(Node a, Node b)
    {
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        //return Math.Sqrt(dx * dx + dy * dy);
        return dx + dy; // Manhatten
    }

    // private List<Node> GetNeighbors(Node node)
    // {
    //     List<Node> neighbors = [];
    //     int[] dx = [-1, 0, 1, 0];
    //     int[] dy = [0, 1, 0, -1];
    //
    //     for (var i = 0; i < 4; i++)
    //     {
    //         var nx = node.X + dx[i];
    //         var ny = node.Y + dy[i];
    //
    //         if (nx >= 0 && nx < Rows && ny >= 0 && ny < Cols)
    //         {
    //             neighbors.Add(_nodes[nx, ny]);
    //         }
    //     }
    //
    //     return neighbors;
    // }

    private List<Node> Predecessors(Node node)
    {
        List<Node> predecessors = [];
        if (node.X - 1 >= 0)
            predecessors.Add(_nodes[node.Y, node.X - 1]);
        if (node.Y - 1 >= 0)
            predecessors.Add(_nodes[node.Y - 1, node.X]);
        return predecessors;
    }

    private List<Node> Successors(Node node)
    {
        List<Node> successors = [];
        if (node.X + 1 < Cols)
            successors.Add(_nodes[node.Y, node.X + 1]);
        if (node.Y + 1 < Rows)
            successors.Add(_nodes[node.Y + 1, node.X]);
        return successors;
    }

    private void ComputeShortestPath()
    {
        while (!_queue.Empty && 
               (!Equals(_goal.Rhs, _goal.G) ||
                _queue.Peek().CompareTo(_goal) < 0))
        {
            var u = _queue.Dequeue();
            if (u.G > u.Rhs)
            {
                u.G = u.Rhs;
                foreach (var s in Successors(u))
                {
                    UpdateVertex(s);
                }
            }
            else
            {
                u.G = double.MaxValue;
                UpdateVertex(u);
                foreach (var s in Successors(u))
                {
                    UpdateVertex(s);
                }
            }
        }
    }

    private void UpdateVertex(Node u)
    {
        if (u != _start)
        {
            u.Rhs = double.MaxValue;
            foreach (var v in Predecessors(u))
            {
                u.Rhs = Math.Min(u.Rhs, v.G + u.Weight);
            }
        }


        if (_queue.Contains(u))
            _queue.Remove(u);

        if (!Equals(u.Rhs, u.G))
            _queue.Enqueue(u);
    }

    public List<Node> GetPath((int y, int x) start, (int y, int x) goal)
    {
        return GetPath(_nodes[start.y, start.x], _nodes[goal.y, goal.x]);
    }
    
    private List<Node> GetPath(Node startNode, Node goalNode)
    {
        var path = new List<Node>();
        var current = goalNode;

        while (current != startNode)
        {
            path.Add(current);
            current = Predecessors(current)
                .First(s => Equals(current.G, s.G + current.Weight));
        }

        path.Add(startNode);
        path.Reverse();
        return path;
    }

    private static bool Equals(double a, double b) => Math.Abs(a - b) < E;
}