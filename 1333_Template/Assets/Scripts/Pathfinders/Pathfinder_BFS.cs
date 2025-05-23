using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : Pathfinder
{
    public override List<GridNode> FindPath(GridNode start, GridNode goal)
    {
        // check if null
        if (start == null || goal == null)
        {
            Debug.LogError("Can't find path: Start or end is null");
            return new List<GridNode>();
        }

        // reset
        foreach (GridNode node in gridManager.GridNodes)
        {
            node.CameFrom = null;
        }

        frontier.Clear();
        visited.Clear();

        // set start
        frontier.Add(start);
        start.CameFrom = null;

        while (frontier.Count > 0)
        {
            GridNode current = FrontierDequeue();

            if (current == goal)
            {
                return RecallPath(start, goal);
            }

            foreach (GridNode next in current.Neighbours)
            {
                if (next != null && !next.IsOccupied() && next.Walkable && !visited.Contains(next))
                {
                    FrontierEnqueue(next);
                    visited.Add(next);
                    next.CameFrom = current;
                }
            }
        }

        return new List<GridNode>();
    }


}
