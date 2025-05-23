using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridSettings gridSettings;
    [SerializeField] private TerrainType defaultTerrainType;
    [SerializeField] private List<TerrainType> terrainTypes;
    public GridSettings GridSettings => gridSettings;

    private GridNode[,] gridNodes;
    public GridNode[,] GridNodes => gridNodes;

#if UNITY_EDITOR
    [Header("Debug for editor playmode only")]
    [SerializeField] private List<GridNode> AllNodes;
#endif

    public bool IsInitialized { get; private set; } = false;

    public void InitializeGrid()
    {
        gridNodes = new GridNode[gridSettings.GridSizeX, gridSettings.GridSizeY];
        AllNodes = new List<GridNode>();

        Debug.Log($"Initializing grid: {gridSettings.GridSizeX}x{gridSettings.GridSizeY}");

        for (int x = 0; x < gridSettings.GridSizeX; x++)
        {
            for(int y = 0; y < gridSettings.GridSizeY; y++)
            {
                Vector3 worldPos = gridSettings.UseXYZPlane
                    ? new Vector3(x, 0, y) * gridSettings.NodeSize
                    : new Vector3(x, y, 0) * gridSettings.NodeSize;

                TerrainType ChosenTerrain = terrainTypes[Random.Range(0, terrainTypes.Count)];

                GridNode node = new GridNode(new Vector2Int(x, y), worldPos, ChosenTerrain);

                gridNodes[x, y] = node;

                AllNodes.Add(node);

            }
        }

        AssignNeighbours();

        IsInitialized = true;
    }

    public GridNode GetNode(int x, int y)
    {
        return gridNodes[x, y];
    }

    public GridNode GetNode(Vector2Int gridPos) => GetNode(gridPos.x, gridPos.y);

    public void SetWalkable(int x, int y, bool walkable)
    {
        GridNode node = GetNode(x, y);
        if (node != null) node.Walkable = walkable;

    }

    private void AssignNeighbours()
    {
        for (int x = 0; x < gridSettings.GridSizeX; x++)
        {
            for (int y = 0; y < gridSettings.GridSizeY; y++)
            {
                GridNode node = gridNodes[x, y];

                node.Neighbours = new GridNode[4];

                if (x > 0)                                  // left
                    node.Neighbours[0] = gridNodes[x - 1, y];
                if (x < gridSettings.GridSizeX - 1)         // right
                    node.Neighbours[1] = gridNodes[x + 1, y];
                if (y > 0)                                  // down
                    node.Neighbours[2] = gridNodes[x, y - 1];
                if (y < gridSettings.GridSizeY - 1)         // up
                    node.Neighbours[3] = gridNodes[x, y + 1];
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (gridNodes == null || gridSettings == null) return;

        for (int x = 0; x < gridSettings.GridSizeX; x++)
        {
            for (int y = 0; y < gridSettings.GridSizeY; y++)
            {
                GridNode node = gridNodes[x, y];
                Gizmos.color = node.Walkable? node.TerrainType.GizmoColor : Color.red;
                Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * gridSettings.NodeSize * 0.9f);
            }
        }
    }

    private void EachNode(System.Action functionToDo)
    {
        for (int x = 0; x < gridSettings.GridSizeX; x++)
        {
            for (int y = 0; y < gridSettings.GridSizeY; y++)
            {
                functionToDo?.Invoke();
            }
        }
    }
}
