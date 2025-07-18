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
        //Debug.Log($"Movement: Setting new target to {targetNode.Name}");

        //Debug.Log($"Combatant: Commanding {unit.name} to node{targetNode.Name}");

        Debug.Log($"Pathfinder null? {pathfinder == null} Unit null? {unit == null}");

        if (pathfinder == null || unit == null) return;

        

        if (targetNode.IsOccupied())
        {
            currentTargetNode = GridManager.Instance.GetClosestReachableNeighbour(unit.CurrentNode, targetNode, pathfinder);
            //Debug.Log($"Movement: Changed target {currentTargetNode.Name}");

            // this may be null if it recursions too much
        }
        else
        {
            currentTargetNode = targetNode;
            //Debug.Log($"Movement: Kept target {currentTargetNode.Name}");
        }

            

        //Debug.Log($"Start node: {unit.CurrentNode?.Name}, End node: {targetNode?.Name}");
        currentPath = pathfinder.FindPath(unit.CurrentNode, currentTargetNode);
        pathIndex = 0;

        //Debug.Log($"Movement: currentPath.Count = {currentPath.Count}, coroutine = {movementCoroutine}");

        if (currentPath.Count == 0)
            Debug.LogWarning("Movement: Path was not found or is empty!");

        if (currentPath.Count > 0)
        {
            //Debug.Log($"Movement: Current path length is {currentPath.Count}");

            if (movementCoroutine != null)
            {
                //Debug.Log($"Movement: Stopping last coroutine");
                StopCoroutine(movementCoroutine);
                isMoving = false;
            }
            else
            {
                //Debug.Log("Movement: No coroutine was running.");
            }

            //Debug.Log($"Movement: Starting new coroutine");
            movementCoroutine = StartCoroutine(WalkPath());
        }
    }

    private IEnumerator WalkPath()
    {
        //Debug.Log($"Movement: Walking path");

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
                    //Debug.Log($"Final node ({currentPath[nextIndex].Name})is occupied. Finding neighbour");

                    GridNode newTarget = GridManager.Instance.GetClosestReachableNeighbour(unit.CurrentNode, nextNode, pathfinder);
                    SetTarget(newTarget);
                    yield break;
                }
                else if (GridManager.Instance.IsFootprintOccupied(nextNode))
                {
                    //Debug.Log($"Next node ({nextNode.Name}) is occupied. Recalculating path.");
                    SetTarget(currentTargetNode);
                    yield break;
                }
            }

            nextWaypoint = currentPath[pathIndex].WorldPosition;

            while (Vector3.Distance(transform.position, nextWaypoint) > 0.05f)
            {
                // Smooth rotation
                Vector3 direction = (nextWaypoint - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4f); 

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
                //Debug.Log($"Path complete! Unit has reached the final destination of {currentTargetNode.Name}");
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
