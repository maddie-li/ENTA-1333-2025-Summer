using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace RTS_1333
{
    public class UnitInstance : BaseUnit, ISelectableObject
    {
        [Header("References")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Pathfinder pathfinder;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3f;

        private List<GridNode> currentPath = new();
        private int pathIndex = 0;
        private bool isMoving = false;

        private Vector3 nextWaypoint;

        public void Initialize(Pathfinder _pathfinder, GridManager _gridManager)
        {
            pathfinder = _pathfinder;
            gridManager = _gridManager;
        }

        private void Update()
        {

        }

        public void SetTarget(GridNode targetNode)
        {
            if (gridManager == null || pathfinder == null || targetNode.CurrentUnit != null) return;

            currentPath = pathfinder.FindPath(CurrentNode, targetNode);
            pathIndex = 0;

            if (currentPath.Count > 0)
            {
                DrawPath();

                if (isMoving)
                    StopCoroutine(WalkPath());

                StartCoroutine(WalkPath());
            }
        }

        private IEnumerator WalkPath()
        {
            isMoving = true;

            while (pathIndex < currentPath.Count)
            {
                nextWaypoint = currentPath[pathIndex].WorldPosition;
                Debug.Log($"Moving to Waypoint {pathIndex}: {nextWaypoint}");

                while (Vector3.Distance(transform.position, nextWaypoint) > 0.05f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                if (pathIndex < currentPath.Count - 1)
                {
                    pathIndex++;
                    UpdateCurrentNode(currentPath[pathIndex]); 
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


}
