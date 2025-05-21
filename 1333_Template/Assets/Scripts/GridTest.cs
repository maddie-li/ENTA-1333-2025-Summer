using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.UIElements;

public class GridTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Pathfinder pathfinder; 

    [Header("Path Settings")]
    [SerializeField] private bool useRandomPositions = true;

    public Vector2Int startPos;
    public Vector2Int goalPos;

    private List<GridNode> path;

    [Header("Visualization")]
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private float markerScale;

    void Start()
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
        }
        else
        {
            // get from inspector
            startNode = gridManager.GetNode(startPos);
            goalNode = gridManager.GetNode(goalPos);
        }

        Debug.Log($"Start Set: {startNode.GridPosition} at {startNode.WorldPosition}");
        Debug.Log($"Goal Set: {goalNode.GridPosition} at {goalNode.WorldPosition}");

        // spawn markers
        SpawnMarker(startPrefab, startNode);
        SpawnMarker(goalPrefab, goalNode);

        // run pathfinding
        path = pathfinder.FindPath(startNode, goalNode);

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("No path :(");
        }
        else
        {
            Debug.Log($"Yes path! {path.Count} nodes long");
        }

    }

    private GridNode GetRandomNode()
    {
        // get dimensions
        int width = gridManager.GridNodes.GetLength(0);
        int height = gridManager.GridNodes.GetLength(1);

        GridNode randomNode = null;

        while (randomNode == null || !randomNode.Walkable)
        {
            int randomX = Random.Range(0, width);
            int randomY = Random.Range(0, height);
            randomNode = gridManager.GetNode(randomX, randomY);
        }

        return randomNode;
    }

    private GameObject SpawnMarker(GameObject prefab, GridNode nodePosition)
    {
        GameObject marker = Instantiate(prefab, nodePosition.WorldPosition, Quaternion.identity, this.transform);
        marker.transform.localScale = Vector3.one * markerScale;
        return marker;
    }


}
