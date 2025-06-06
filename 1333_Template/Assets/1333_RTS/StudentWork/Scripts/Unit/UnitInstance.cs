using UnityEngine;
using System.Collections.Generic;

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

        public void Initialize(Pathfinder _pathfinder, GridManager _gridManager)
        {
            pathfinder = _pathfinder;
            gridManager = _gridManager;
        }

        private void Update()
        {
            if (!isMoving || currentPath.Count == 0 || pathIndex >= currentPath.Count) return;

            Vector3 nextWaypoint = currentPath[pathIndex].WorldPosition;
            float step = moveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, step);

            if (Vector3.Distance(transform.position, nextWaypoint) < 0.05f)
            {
                pathIndex++;
                isMoving = pathIndex < currentPath.Count;
            }
        }

        public void SetTarget(GridNode targetNode)
        {
            if (gridManager == null || pathfinder == null) return;

            if (targetNode.CurrentUnit != null) return;

            currentPath = pathfinder.FindPath(CurrentNode, targetNode);
            pathIndex = 0;
            isMoving = currentPath.Count > 0;

            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                UpdateCurrentNode(currentPath[i]);
                Debug.DrawLine(currentPath[i].WorldPosition, currentPath[i + 1].WorldPosition, Color.red, 5f);

            }
        }
    }
}
