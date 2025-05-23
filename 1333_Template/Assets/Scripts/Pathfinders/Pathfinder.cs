using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    [SerializeField] protected GridManager gridManager;
    protected List<GridNode> frontier = new List<GridNode>();
    protected Dictionary<GridNode, GridNode> visitedFrom = new Dictionary<GridNode, GridNode>();
    
    public abstract List<GridNode> FindPath(GridNode start, GridNode goal); 
    public List<GridNode> RecallPath(GridNode start, GridNode goal)
    {
        GridNode current = goal;
        List<GridNode> path = new List<GridNode>();

        while (current != null && current != start)
        {
            path.Add(current);
            current = current.CameFrom;
            //current = visitedFrom[current];
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

    /*public void FrontierPriorityEnqueue(GridNode node)
    {
        frontier.Add(node);
    }

    public GridNode FrontierPriorityDequeue()
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
    }*/
}
