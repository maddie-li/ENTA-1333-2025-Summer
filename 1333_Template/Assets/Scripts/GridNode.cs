using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridNode
{
    public string Name;
    public Vector3 WorldPosition;       // position in 3D space
    public Vector2Int GridPosition;     // grid coordinates

    [Header("Properties")]
    public TerrainType TerrainType;
    public bool Walkable;
    public int Weight;

    [Header("Pathfinding")]
    public float gCost;                  // distance from start
    public float hCost;                  // estimated distance to goal
    public float fCost => gCost + hCost; // total estimated cost

    public GridNode CameFrom;            // previous node

    [Header("Neighbours")]
    public GridNode[] Neighbours = new GridNode[4];

    public GridNode(Vector2Int gridPos, Vector3 worldPos, TerrainType terrain)
    {
        Name = $"Cell_{gridPos.x}_{gridPos.y}";
        GridPosition = gridPos;
        WorldPosition = worldPos;
        TerrainType = terrain;
        Walkable = terrain.IsWalkable;
        Weight = terrain.MovementCost;
    }
}
