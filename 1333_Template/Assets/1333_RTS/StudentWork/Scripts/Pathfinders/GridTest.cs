using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{

    public class GridTest : MonoBehaviour
    {
        [Header("References")]


        [Header("Pathfinder")]
        private Pathfinder[] pathfinders;

        private enum SelectedPathfinder
        {
            BreadthFirst,
            Dijkstra,
            AStar,
            All
        }

        [SerializeField] private SelectedPathfinder selectedPathfinder;

        [SerializeField] private bool useRandomPositions = true;

        [Header("Path Settings")]

        public Vector2Int startPos;
        public Vector2Int goalPos;

        private List<List<GridNode>> allPaths = new List<List<GridNode>>();

        [Header("Visualization")]
        [SerializeField] private GameObject startPrefab;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private float markerScale;

        private GameObject startMarker;
        private GameObject goalMarker;

        private void Awake()
        {
            pathfinders = GetComponentsInChildren<Pathfinder>();

            //GenerateGridTest();
        }

        private void OnValidate()
        {
            //GenerateGridTest();
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateGridTest();
            }
        }

        void GenerateGridTest()
        {
            // set random
            GridNode startNode = GetRandomNode();
            GridNode goalNode = GetRandomNode();

            if (useRandomPositions)
            {
                // random nodes
                startNode = GetRandomNode();
                goalNode = GetRandomNode();
                // make sure they are not the same
                while (goalNode == startNode)
                {
                    goalNode = GetRandomNode();
                }

                startPos = startNode.GridPosition;
                goalPos = goalNode.GridPosition;
            }
            else
            {
                // get from inspector
                startNode = GridManager.Instance.GetNode(startPos);
                goalNode = GridManager.Instance.GetNode(goalPos);
            }

            //Debug.Log($"Start Set: {startNode.GridPosition} at {startNode.WorldPosition}");
            //Debug.Log($"Goal Set: {goalNode.GridPosition} at {goalNode.WorldPosition}");

            // spawn markers
            SpawnMarker(startPrefab, startNode, ref startMarker);
            SpawnMarker(goalPrefab, goalNode, ref goalMarker);

            allPaths.Clear();

            switch (selectedPathfinder)
            {
                case SelectedPathfinder.BreadthFirst:

                    RunPathfinding(pathfinders[0], startNode, goalNode);
                    break;

                case SelectedPathfinder.Dijkstra:

                    RunPathfinding(pathfinders[1], startNode, goalNode);
                    break;

                case SelectedPathfinder.AStar:

                    RunPathfinding(pathfinders[2], startNode, goalNode);
                    break;

                default:

                    for (int i = 0; i < pathfinders.Length; i++)
                    {
                        RunPathfinding(pathfinders[i], startNode, goalNode);
                    }
                    break;
            }

        }

        private void RunPathfinding(Pathfinder pathfinder, GridNode startNode, GridNode goalNode)
        {
            List<GridNode> path = pathfinder.FindPath(startNode, goalNode);
            allPaths.Add(path);

            if (path == null || path.Count == 0)
            {
                //Debug.LogWarning($"{pathfinder.GetType().Name} found no path :)");
            }
            else
            {
                //Debug.Log($"{pathfinder.GetType().Name} found a path! {path.Count} nodes long");
            }
        }

        private GridNode GetRandomNode()
        {
            // get dimensions
            int width = GridManager.Instance.GridNodes.GetLength(0);
            int height = GridManager.Instance.GridNodes.GetLength(1);

            GridNode randomNode = null;

            while (randomNode == null || !randomNode.Walkable)
            {
                int randomX = UnityEngine.Random.Range(0, width);
                int randomY = UnityEngine.Random.Range(0, height);
                randomNode = GridManager.Instance.GetNode(randomX, randomY);
            }

            return randomNode;
        }

        private GameObject SpawnMarker(GameObject prefab, GridNode nodePosition, ref GameObject marker)
        {
            if (marker != null)
            {
                Destroy(marker);
            }

            marker = Instantiate(prefab, nodePosition.WorldPosition, Quaternion.identity, this.transform);
            marker.transform.localScale = Vector3.one * markerScale;

            return marker;
        }

        private void OnDrawGizmos()
        {
            if (allPaths != null || allPaths.Count != 0)
            {
                for (int i = 0; i < allPaths.Count; i++)
                {
                    List<GridNode> path = allPaths[i];

                    Gizmos.color = pathfinders[i].PathColor;

                    for (int j = 0; j < path.Count - 1; j++)
                    {
                        Gizmos.DrawLine(path[j].WorldPosition, path[j + 1].WorldPosition);
                    }
                }
            }

        }

    }

}