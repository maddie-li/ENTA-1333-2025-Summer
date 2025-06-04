using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

public class UnitInstance : BaseUnit
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;

    [Header("Prefab")]
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private GameObject unitSkins;
    [SerializeField] private ParticleSystem hurtParticles;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;

    private Pathfinder pathfinder;
    private List<GridNode> currentPath = new();
    private int pathIndex = 0;

    private Vector3? targetWorldPosition = null;
    private bool isMoving = false;

    public bool IsMoving => isMoving;
    public List<GridNode> CurrentPath => currentPath;

    public void Initialize(Pathfinder _pathfinder, UnitType _unitType)
    {
        pathfinder = _pathfinder;
        unitType = _unitType;

    }

    private void Update()
    {
        if (!isMoving || currentPath == null || currentPath.Count == 0 || pathIndex >= currentPath.Count)
            return;

        // get next
        Vector3 nextWaypoint = currentPath[pathIndex].WorldPosition;
        // go to next
        Vector3 direction = (nextWaypoint - transform.position).normalized;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, step);

        //check if there
        if (Vector3.Distance(transform.position, nextWaypoint) < 0.05f)
        {
            pathIndex++;

            if (pathIndex >= currentPath.Count)
            {
                isMoving = false;
            }
        }
    }

    public void SetTarget(Vector3 worldPosition)
    {
        targetWorldPosition = worldPosition;
        
        if (gridManager.GetNodeFromWorldPosition(worldPosition).CurrentUnit != null)
        {
            Debug.Log("Move failed, target node is occupied");
            return;

        }
        // pathfind
        currentPath = pathfinder.FindPath(gridManager.GetNodeFromWorldPosition(transform.position), gridManager.GetNodeFromWorldPosition(worldPosition));
        pathIndex = 0;
        isMoving = currentPath != null && currentPath.Count > 1;
    }
    public void SetTarget(GridNode node)
    {
        SetTarget(node.WorldPosition);
    }
    public override void MoveTo(GridNode targetNode)
    {
        SetTarget(targetNode);
    }
}
