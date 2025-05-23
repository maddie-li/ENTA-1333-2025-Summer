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

        frontier.Clear();
        visitedFrom.Clear();

        // set start
        frontier.Add(start);

        while (frontier.Count > 0)
        {
            GridNode current = FrontierDequeue();

            if (current == goal)
            {
                return RecallPath(start, goal);
            }

            foreach (GridNode next in current.Neighbours)
            {
                if (IsWalkable(next))
                {
                    FrontierEnqueue(next);
                    visitedFrom.Add(next, current);
                }
            }
        }

        return new List<GridNode>();
    }


}
