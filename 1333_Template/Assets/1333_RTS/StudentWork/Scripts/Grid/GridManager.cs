using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;


namespace RTS_1333
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Camera cam;

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

        public static GridManager Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public bool IsInitialized { get; private set; } = false;

        public void InitializeGrid()
        {
            gridNodes = new GridNode[gridSettings.GridSizeX, gridSettings.GridSizeY];
            AllNodes = new List<GridNode>();

            Debug.Log($"Initializing grid: {gridSettings.GridSizeX}x{gridSettings.GridSizeY}");

            for (int x = 0; x < gridSettings.GridSizeX; x++)
            {
                for (int y = 0; y < gridSettings.GridSizeY; y++)
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
            if (x < 0 || y < 0 || x >= gridSettings.GridSizeX || y >= gridSettings.GridSizeY)
            {
                Debug.LogWarning($"GetNode out of bounds: ({x}, {y}) not inside grid size ({gridSettings.GridSizeX}, {gridSettings.GridSizeY})");
                return null;
            }
            return gridNodes[x, y];
        }

        public GridNode GetNode(Vector2Int gridPos) => GetNode(gridPos.x, gridPos.y);

        public GridNode GetNodeFromWorldPosition(Vector3 position)
        {
            int x = gridSettings.UseXYZPlane
                ? Mathf.RoundToInt(position.x / gridSettings.NodeSize)
                : Mathf.RoundToInt(position.x / gridSettings.NodeSize);

            int y = gridSettings.UseXYZPlane
                ? Mathf.RoundToInt(position.z / gridSettings.NodeSize)
                : Mathf.RoundToInt(position.y / gridSettings.NodeSize); 

            x = Mathf.Clamp(x, 0, gridSettings.GridSizeX - 1);
            y = Mathf.Clamp(y, 0, gridSettings.GridSizeY - 1);

            //Debug.Log($"Converted World Position {position} to  Grid Position ({x}, {y})"); 

            return GetNode(x, y);
        }

        public GridNode GetNodeFromMousePosition()
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane ground = new(Vector3.up, Vector3.zero);

            if (ground.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                GridNode node =GetNodeFromWorldPosition(hitPoint);

                //Debug.Log("Got node from mouse position!");
                return node;
            }
            else
            {
                Debug.Log("DID NOT GET node from mouse position!");
                return null;
            }
        }


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

        public bool IsFootprintOccupied(GridNode startnode)
        {
            return IsFootprintOccupied(startnode, 1, 1);
        }

        public bool IsFootprintOccupied(GridNode startNode, int width, int length)
        {
            //Debug.Log($"Checking {width}x{length}");

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    var node = GetNode(startNode.GridPosition.x + x, startNode.GridPosition.y + y);

                    if (node == null)
                    {
                        //Debug.LogWarning($"Node is null at ({startNode.GridPosition.x + x},{ startNode.GridPosition.y + y})");
                        return true;
                    }
                    if (node.CurrentUnit != null)
                    {
                        //Debug.Log($"Node {node.Name} is occupied by {node.CurrentUnit.Name}");
                        return true;
                    }
                    if (!node.Walkable)
                    {
                        //Debug.Log($"Node {node.Name} is not walkable");
                        return true;
                    }
                    else
                    {
                        //Debug.Log($"Node {node.Name} is AVAILABLE");
                    }
                }
            }
            return false;
        }

        public void FootprintOccupy(GridNode startNode, int width, int length, BaseUnit unit)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    var node = GetNode(startNode.GridPosition.x + x, startNode.GridPosition.y + y);
                    if (node != null)
                    {
                        node.CurrentUnit = unit;
                    }
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
                    Gizmos.color = node.Walkable ? node.TerrainType.GizmoColor : Color.red;
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
}