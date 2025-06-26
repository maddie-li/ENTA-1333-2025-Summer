using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_1333;

public class MovementController : MonoBehaviour
{
    //[Header("References")]
    private Pathfinder pathfinder;
    private Unit unit;

    //[Header("Movement")]
    private float moveSpeed = 3f;
    private bool isMoving;
    private List<GridNode> currentPath = new();
    private int pathIndex = 0;
    private Vector3 nextWaypoint;
    private Coroutine movementCoroutine;
    private GridNode currentTargetNode;

    public void Initialize(Pathfinder _pathfinder, Unit _unit, float _moveSpeed)
    {
        pathfinder = _pathfinder;
        unit = _unit;
        moveSpeed = _moveSpeed;
    }

    public void SetTarget(GridNode targetNode)
    {
        if (pathfinder == null || unit == null) return;

        currentTargetNode = targetNode;

        currentPath = pathfinder.FindPath(unit.CurrentNode, targetNode);
        pathIndex = 0;

        if (currentPath.Count > 0)
        {
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
                isMoving = false;
            }

            movementCoroutine = StartCoroutine(WalkPath());
        }
    }

    private IEnumerator WalkPath()
    {
        DrawPath();
        isMoving = true;

        while (pathIndex < currentPath.Count)
        {
            int nextIndex = pathIndex + 1;

            if (nextIndex < currentPath.Count)
            {
                GridNode nextNode = currentPath[nextIndex];

                if (nextIndex == currentPath.Count - 1 && GridManager.Instance.IsFootprintOccupied(nextNode))
                {
                    Debug.Log($"Final node ({currentPath[nextIndex].Name})is occupied. Finding neighbour");

                    GridNode newTarget = GridManager.Instance.GetClosestReachableNeighbour(unit.CurrentNode, nextNode, pathfinder);
                    SetTarget(newTarget);
                    yield break;
                }
                else if (GridManager.Instance.IsFootprintOccupied(nextNode))
                {
                    Debug.Log($"Next node ({nextNode.Name}) is occupied. Recalculating path.");
                    SetTarget(currentTargetNode);
                    yield break;
                }
            }

            nextWaypoint = currentPath[pathIndex].WorldPosition;

            while (Vector3.Distance(transform.position, nextWaypoint) > 0.05f)
            {
                transform.LookAt(nextWaypoint);
                transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }

            if (pathIndex < currentPath.Count - 1)
            {
                pathIndex++;
                unit.UpdateCurrentNode(currentPath[pathIndex]);
            }
            else
            {
                Debug.Log("Path complete! Unit has reached the final destination.");
                break;
            }
        }

        isMoving = false;
    }

    private void DrawPath()
    {
        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            Debug.DrawLine(currentPath[i].WorldPosition, currentPath[i + 1].WorldPosition, Color.red, 5f);
        }
    }
}
