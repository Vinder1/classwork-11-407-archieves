// using Algo.PriorityQueue;
// using AStar.Nodes;
//
// namespace Algo.Pathfinding;
//
// using System;
// using System.Collections.Generic;
//
// // Узел графа
// public class DNode : IComparable<Node>
// {
//     public int X { get; init; }
//     public int Y { get; init; }
//     public int Weight { get; set; }
//     public Node Goal { get; }
//
//     public double g { get; set; } // Стоимость от начального узла до текущего
//     public double rhs { get; set; } // Односторонняя оценка
//
//     public DNode(int x, int y, int weight, Node? goal=null)
//     {
//         X = x;
//         Y = y;
//         Weight = weight;
//         Goal = goal ?? this;
//         g = double.MaxValue;
//         rhs = double.MaxValue;
//     }
//
//     public int CompareTo(Node? other)
//     {
//         if (other is null)
//             return 1;
//         
//         var key1 = Math.Min(g, rhs) + DStar.Heuristic(this, Goal);
//         var key2 = Math.Min(other.g, other.rhs) + DStar.Heuristic(other, Goal);
//
//         if (Equals(key1, key2))
//             return key1.CompareTo(key2);
//
//         var key1_2 = Math.Min(g, rhs);
//         var key2_2 = Math.Min(other.g, other.rhs);
//         return key1_2.CompareTo(key2_2);
//     }
// }
//
// public class DStar
// {
//     private int Rows { get; }
//     private int Cols { get; }
//     
//     private readonly DNode[,] _nodes;
//     private readonly MyPriorityQueue<DNode> _queue = new ();
//     private DNode _start = null!;
//     private DNode _goal = null!;
//     
//     private const double E = 1.0e-10;
//
//     public DStar(int rows, int cols, (int y, int x) start, (int y, int x) goal)
//     {
//         Rows = rows;
//         Cols = cols;
//         _nodes = new DNode[Rows, Cols];
//         InitializeGraph(start, goal);
//     }
//     
//     public LpaStar(GridNode[,] grid, (int y, int x) start, (int y, int x) goal)
//     {
//         Rows = grid.GetLength(0);
//         Cols = grid.GetLength(1);
//         _nodes = new Node[Rows, Cols];
//         InitializeGraph(start, goal, grid);
//     }
//
//     public void Run()
//     {
//         ComputeShortestPath();
//     }
//
//     public void Print()
//     {
//         // Console.WriteLine("Путь:");
//         // foreach (var node in GetPath(_start, _goal))
//         // {
//         //     Console.WriteLine($"({node.X}, {node.Y})");
//         // }
//         var matrix = new char[Rows, Cols];
//         for (var y = 0; y < Rows; y++)
//         {
//             for (var x = 0; x < Cols; x++)
//             {
//                 matrix[y,x] = _nodes[y, x].Weight == 1 ? ' ' : '\u25a0';
//             }
//         }
//         foreach (var node in GetPath(_start, _goal))
//         {
//             matrix[node.Y, node.X] = '*';
//         }
//
//         for (var y = Rows-1; y >= 0; y--)
//         {
//             for (var x = 0; x < Cols; x++)
//             {
//                 Console.Write(matrix[y, x]);
//             }
//             Console.WriteLine();
//         }
//         Console.WriteLine();
//     }
//     
//     public void Update((int y, int x, int weight)[] updatedNodes)
//     {
//         foreach (var node in updatedNodes)
//         {
//             _nodes[node.y, node.x].Weight = node.weight;
//             UpdateVertex(_nodes[node.y, node.x]);
//         }
//         ComputeShortestPath();
//     }
//
//     private void InitializeGraph((int y, int x) start, (int y, int x) goal, GridNode[,]? grid=null)
//     {
//         _goal = _nodes[goal.y, goal.x] 
//             = new Node(goal.x, goal.y, 1);
//         if (grid == null)
//         {
//             for (var x = 0; x < Cols; x++)
//             {
//                 for (var y = 0; y < Rows; y++)
//                 {
//                     if (x == goal.x && y == goal.y)
//                         continue;
//                     _nodes[y, x] = new Node(x, y, 1, _goal);
//                 }
//             }
//         }
//         else
//         {
//             for (var x = 0; x < Cols; x++)
//             {
//                 for (var y = 0; y < Rows; y++)
//                 {
//                     if (x == goal.x && y == goal.y)
//                         continue;
//                     _nodes[y, x] = new Node(x, y, (int)grid[y,x].Weight, _goal);
//                 }
//             }
//         }
//         
//         _start = _nodes[start.y, start.x];
//         
//         _start.rhs = 0;
//         _queue.Enqueue(_start);
//     }
//
//     public static double Heuristic(Node a, Node b)
//     {
//         return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); // Manhatten
//     }
//
//     // private List<Node> GetNeighbors(Node node)
//     // {
//     //     List<Node> neighbors = [];
//     //     int[] dx = [-1, 0, 1, 0];
//     //     int[] dy = [0, 1, 0, -1];
//     //
//     //     for (var i = 0; i < 4; i++)
//     //     {
//     //         var nx = node.X + dx[i];
//     //         var ny = node.Y + dy[i];
//     //
//     //         if (nx >= 0 && nx < Rows && ny >= 0 && ny < Cols)
//     //         {
//     //             neighbors.Add(_nodes[nx, ny]);
//     //         }
//     //     }
//     //
//     //     return neighbors;
//     // }
//
//     private List<Node> Predecessors(Node node)
//     {
//         List<Node> predecessors = [];
//         if (node.X - 1 >= 0)
//             predecessors.Add(_nodes[node.Y, node.X - 1]);
//         if (node.Y - 1 >= 0)
//             predecessors.Add(_nodes[node.Y - 1, node.X]);
//         return predecessors;
//     }
//     
//     private List<Node> Successors(Node node)
//     {
//         List<Node> successors = [];
//         if (node.X + 1 < Cols)
//             successors.Add(_nodes[node.Y, node.X + 1]);
//         if (node.Y + 1 < Rows)
//             successors.Add(_nodes[node.Y + 1, node.X]);
//         return successors;
//     }
//
//     private void ComputeShortestPath()
//     {
//         while (!_queue.Empty)
//         {
//             var u = _queue.Dequeue();
//             if (u.g > u.rhs)
//             {
//                 u.g = u.rhs;
//                 foreach (var s in Successors(u))
//                 {
//                     UpdateVertex(s);
//                 }
//             }
//             else
//             {
//                 u.g = double.MaxValue;
//                 var neighbors = Successors(u);
//                 neighbors.Add(u);
//                 foreach (var s in neighbors)
//                 {
//                     UpdateVertex(s);
//                 }
//             }
//         }
//     }
//
//     private void UpdateVertex(Node u)
//     {
//         u.rhs = double.MaxValue;
//         foreach (var v in Predecessors(u))
//         {
//             u.rhs = Math.Min(u.rhs, v.g + u.Weight);
//         }
//
//         if (_queue.Contains(u))
//             _queue.Remove(u);
//
//         if (Math.Abs(u.rhs - u.g) > E)
//             _queue.Enqueue(u);
//     }
//
//     private List<Node> GetPath(Node startNode, Node goalNode)
//     {
//         var path = new List<Node>();
//         var current = goalNode;
//         
//         while (current != startNode)
//         {
//             path.Add(current);
//             current = Predecessors(current)
//                 .Where(s => Equals(current.g, s.g + current.Weight))
//                 .OrderBy(s => s.g).First();
//         }
//         path.Add(startNode);
//         path.Reverse();
//         return path;
//     }
//     
//     private static bool Equals(double a, double b) => Math.Abs(a - b) < E;
// }