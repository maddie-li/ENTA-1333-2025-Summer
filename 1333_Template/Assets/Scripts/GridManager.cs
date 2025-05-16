using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridSettings gridSettings;
    [SerializeField] private TerrainType defaultTerrainType;
    public GridSettings GridSettings => gridSettings;

    private GridNode[,] gridNodes;

#if UNITY_EDITOR
    [Header("Debug for editor playmode only")]
    [SerializeField] private List<GridNode> AllNodes;
#endif

    public bool IsInitialized { get; private set; } = false;

    public void InitializeGrid()
    {
        gridNodes = new GridNode[gridSettings.GridSizeX, gridSettings.GridSizeY];
        AllNodes = new List<GridNode>();

        for(int x = 0; x < gridSettings.GridSizeX; x++)
        {
            for(int y = 0; y < gridSettings.GridSizeY; y++)
            {
                Vector3 worldPos = gridSettings.UseXYZPlane
                    ? new Vector3(x, 0, y) * gridSettings.NodeSize
                    : new Vector3(x, y, 0) * gridSettings.NodeSize;

                GridNode node = new GridNode
                {
                    Name = $"Cell_{x}_{y}",
                    WorldPosition = worldPos,
                    TerrainType = defaultTerrainType,
                    Walkable = true,
                    Weight = 1
                };
                gridNodes[x, y] = node;
                AllNodes.Add(node);

            }
        }

        IsInitialized = true;
    }

    /*public GridNode GetNode(int x, int y)
    {

    }*/

    /*public void SetWalkable(int x, int y, bool walkable)
    {

    }*/

    private void OnDrawGizmos()
    {
        if (gridNodes == null || gridSettings == null) return;

        for (int x = 0; x < gridSettings.GridSizeX; x++)
        {
            for (int y = 0; y < gridSettings.GridSizeY; y++)
            {
                GridNode node = gridNodes[x, y];
                Gizmos.color = Color.green;
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
