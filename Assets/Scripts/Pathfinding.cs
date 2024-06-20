using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathNode
{
    public int X { get; }
    public int Y { get; }
    public bool IsObstacle { get; private set; }
    public List<PathNode> Neighbors { get; set; }
    public PathNode Parent { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public PathNode(int y, int x, bool isObstacle = false)
    {
        X = x;
        Y = y;
        IsObstacle = isObstacle;
        Neighbors = new List<PathNode>();
    }
    public void SetObstacle()
    {
        IsObstacle = true;
    }
    public void ResetObstacle()
    {
        IsObstacle = false;
    }
}
public class Grid
{
    public int Rows { get; }
    public int Columns { get; }
    public PathNode[,] Nodes { get; }
    public Grid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Nodes = new PathNode[rows, columns];

        InitializeNodes();
        InitializeNeighbors();
    }
    private void InitializeNodes()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                Nodes[row, col] = new PathNode(row, col);
            }
        }
    }
    private void InitializeNeighbors()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                Nodes[row, col].Neighbors = GetNeighbors(row, col);
            }
        }
    }
    private List<PathNode> GetNeighbors(int row, int col)
    {
        List<PathNode> neighbors = new List<PathNode>();

        if (row > 0) neighbors.Add(Nodes[row - 1, col]);
        if (row < Rows - 1) neighbors.Add(Nodes[row + 1, col]);
        if (col > 0) neighbors.Add(Nodes[row, col - 1]);
        if (col < Columns - 1) neighbors.Add(Nodes[row, col + 1]);

        return neighbors;
    }
    public PathNode this[int row, int col]
    {
        get { return Nodes[row, col]; }
    }
}
public class Pathfinding
{
    public static List<PathNode> FindPath(Grid grid, int startX, int startY, int targetX, int targetY)
    {
        var startNode = grid.Nodes[startY, startX];
        var targetNode = grid.Nodes[targetY, targetX];

        List<PathNode> open = new List<PathNode>();
        HashSet<PathNode> closed = new HashSet<PathNode>();

        open.Add(startNode);
        while (open.Count > 0)
        {
            PathNode current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].FCost < current.FCost)
                {
                    current = open[i];
                }
            }

            open.Remove(current);
            closed.Add(current);

            if (current == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (var neighbor in current.Neighbors)
            {
                if (neighbor.IsObstacle || closed.Contains(neighbor))
                {
                    continue;
                }

                neighbor.GCost = current.GCost + 1;
                neighbor.HCost = CalculateDistance(targetNode, neighbor);
                neighbor.Parent = current;
                if (!open.Contains(neighbor))
                {
                    open.Add(neighbor);
                }
            }
        }
        return null;
    }
    private static List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }
    private static int CalculateDistance(PathNode a, PathNode b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}