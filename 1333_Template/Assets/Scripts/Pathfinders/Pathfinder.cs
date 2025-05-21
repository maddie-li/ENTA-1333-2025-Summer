using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    [SerializeField] protected GridManager gridManager;
    protected Queue<GridNode> frontier = new Queue<GridNode>();
    protected HashSet<GridNode> visited = new HashSet<GridNode>();
    
    public abstract List<GridNode> FindPath(GridNode start, GridNode goal); 
    public List<GridNode> RecallPath(GridNode start, GridNode goal)
    {
        GridNode current = goal;
        List<GridNode> path = new List<GridNode>();

        while (current != null && current != start)
        {
            path.Add(current);
            current = current.CameFrom;
        }

        path.Add(start);
        path.Reverse();

        return path;
    }
}
