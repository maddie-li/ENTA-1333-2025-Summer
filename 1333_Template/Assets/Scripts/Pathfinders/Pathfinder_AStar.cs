using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Pathfinder
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
        frontier.Clear();
        visitedFrom.Clear();
        costSoFar.Clear();

        // set start
        frontier.Add(start);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            GridNode current = FrontierPriorityDequeue();

            if (current == goal)
            {
                return RecallPath(start, goal);
            }

            foreach (GridNode next in current.Neighbours)
            {

                if (IsWalkable(next))
                {
                    int newCost = costSoFar[current] + next.Weight + Distance(next,goal);

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        visitedFrom.Add(next, current);
                        FrontierEnqueue(next);
                    }
                    
                }
                
            }
        }

        return new List<GridNode>();
    }
}