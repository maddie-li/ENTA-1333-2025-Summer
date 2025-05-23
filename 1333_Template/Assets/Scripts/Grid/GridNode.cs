using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [Header("Neighbours")]
    public GridNode[] Neighbours = new GridNode[4];

    [Header("Occupancy")]
    public Unit CurrentUnit;

    public GridNode(Vector2Int gridPos, Vector3 worldPos, TerrainType terrain)
    {
        Name = $"Cell_{gridPos.x}_{gridPos.y}";
        GridPosition = gridPos;
        WorldPosition = worldPos;
        TerrainType = terrain;
        Walkable = terrain.IsWalkable;
        Weight = terrain.MovementCost;
    }

    public bool IsOccupied()
    {
        return CurrentUnit != null;
    }
}
