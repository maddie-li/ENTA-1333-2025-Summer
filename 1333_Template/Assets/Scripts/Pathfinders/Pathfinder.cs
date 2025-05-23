using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    [SerializeField] protected GridManager gridManager;
    protected List<GridNode> frontier = new List<GridNode>();
    protected Dictionary<GridNode, GridNode> visitedFrom = new Dictionary<GridNode, GridNode>();
    protected Dictionary<GridNode, int> costSoFar = new Dictionary<GridNode, int>();
    
    public abstract List<GridNode> FindPath(GridNode start, GridNode goal); 

    public bool IsWalkable(GridNode node)
    {
        return (node != null && !node.IsOccupied() && node.Walkable && !visitedFrom.ContainsKey(node));
    }

    public List<GridNode> RecallPath(GridNode start, GridNode goal)
    {
        GridNode current = goal;
        List<GridNode> path = new List<GridNode>();

        while (current != null && current != start)
        {
            path.Add(current);
            current = visitedFrom[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }

    public void FrontierEnqueue(GridNode node)
    {
        frontier.Add(node);
    }

    public GridNode FrontierDequeue()
    {
        GridNode first;

        if (frontier.Count == 0)
        {
            Debug.Log("Unable to dequeue - frontier empty");
            return null;
        }

        first = frontier[0]; 
        frontier.RemoveAt(0);

        return first;
    }

    public GridNode FrontierPriorityDequeue()
    {
        GridNode lowest;

        if (frontier.Count == 0)
        {
            Debug.Log("Unable to dequeue - frontier empty");
            return null;
        }

        frontier.Sort((a, b) => costSoFar[a].CompareTo(costSoFar[b]));
        lowest = frontier[0];
        frontier.RemoveAt(0);

        return lowest;
    }

    public int Distance(GridNode a, GridNode b)
    {
        return Mathf.Abs(b.GridPosition.x - a.GridPosition.x) + Mathf.Abs(b.GridPosition.y - a.GridPosition.y);
    }
}
