using AStar.Nodes;

namespace AStar.Algorithms
{
    public static class GridAStarWithWeights
    {
        public static List<GridNode>? FindPath(GridNode[,] grid, GridNode start, GridNode end)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            HashSet<GridNode> closedSet = new HashSet<GridNode>();
            PriorityQueue<GridNode, float> openSet = new PriorityQueue<GridNode, float>();

            start.GCost = 0;
            start.HCost = Heuristic(start, end);
            openSet.Enqueue(start, start.FCost);

            while (openSet.Count > 0)
            {
                GridNode current = openSet.Dequeue();

                if (current == end)
                    return ReconstructPath(start, end);

                closedSet.Add(current);

                foreach (GridNode neighbor in GetNeighbors(current, grid, rows, cols))
                {
                    if (!neighbor.Walkable || closedSet.Contains(neighbor))
                        continue;

                    float tempGCost = current.GCost + Heuristic(current, neighbor) * neighbor.Weight;

                    if (tempGCost < neighbor.GCost)
                    {
                        neighbor.GCost = tempGCost;
                        neighbor.HCost = Heuristic(neighbor, end);
                        neighbor.Parent = current;

                        openSet.Enqueue(neighbor, neighbor.FCost);
                    }
                }
            }

            return null;
        }

        private static List<GridNode> ReconstructPath(GridNode start, GridNode end)
        {
            List<GridNode> path = new List<GridNode>();
            GridNode current = end;

            while (current != start)
            {
                path.Add(current!);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }

        private static float Heuristic(GridNode a, GridNode b)
        {
            int dx = Math.Abs(a.X - b.X);
            int dy = Math.Abs(a.Y - b.Y);
            
            //return (float)Math.Sqrt(dx * dx + dy * dy);
            return (dx + dy); //(float)(Math.Sqrt(2) - 2) * Math.Min(dx, dy);
        }

        private static List<GridNode> GetNeighbors(GridNode node, GridNode[,] grid, int rows, int cols)
        {
            List<GridNode> neighbors = new List<GridNode>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;
                    
                    // PRAVKA
                    if (i != 0 && j != 0) continue;
                    
                    int newX = node.X + i;
                    int newY = node.Y + j;

                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                    {
                        neighbors.Add(grid[newX, newY]);
                    }
                }
            }

            return neighbors;
        }
    }
}

namespace AStar.Nodes
{
    public class GridNode : Node<int>
    {
        public GridNode(int x, int y) : base(x, y)
        {
        }

        public GridNode Parent { get; set; }
    }

    public abstract class Node<T> : IComparable<Node<T>>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public bool Walkable { get; set; } = true;

        public float Weight { get; set; }

        public float GCost { get; set; } = float.MaxValue;
        public float HCost { get; set; }

        public float FCost => GCost + HCost;

        public Node(T x, T y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(Node<T>? otherNode)
        {
            return FCost.CompareTo(otherNode!.FCost);
        }
    }
}