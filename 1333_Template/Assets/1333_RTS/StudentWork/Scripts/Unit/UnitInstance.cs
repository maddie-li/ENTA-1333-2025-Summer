using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace RTS_1333
{
    public class UnitInstance : BaseUnit, ISelectableObject
    {
        public bool IsAttacking;

        [Header("References")]
        [SerializeField] private Pathfinder pathfinder;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3f;
        private bool isMoving;
        private List<GridNode> currentPath = new();
        private int pathIndex = 0;
        private Vector3 nextWaypoint; 
        private Coroutine movementCoroutine;

        [Header("Visuals")]
        private Renderer[] renderers;
        private Material defaultMat;
        private Material selectedMat;



        private void Update()
        {
            if(IsAttacking)
            {
                Debug.Log($"Attacking {GetClosestEnemy()} at {GetClosestEnemy().CurrentNode.Name}");
                SetTarget(GetClosestEnemy().CurrentNode);
            }

        }

        public void Initialize(Pathfinder _pathfinder)
        {
            pathfinder = _pathfinder;
        }

        public void SetupMat(Material selected)
        {
            Debug.Log("Setting up materials");
            renderers = GetComponentsInChildren<Renderer>();
            Debug.Log(renderers.Length);
            defaultMat = GetComponentInChildren<Renderer>().material;

            selectedMat = selected;
        }

        private UnitInstance GetClosestEnemy()
        {
            UnitInstance closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            Vector3 myPos = transform.position;

            foreach (UnitInstance enemy in GetEnemies())
            {
                float distance = Vector3.Distance(myPos, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        private List<UnitInstance> GetEnemies()
        {
            return UnitManager.Instance.unitsByArmy[Army.Enemy];
        }

        public void SetTarget(Transform transform)
        {
            GridNode targetNode = GridManager.Instance.GetNodeFromWorldPosition(transform.position);
            SetTarget(targetNode);
        }

        public void SetTarget(GridNode targetNode)
        {
            if (pathfinder == null) return;

            if (targetNode.CurrentUnit != null)
            {
                targetNode = GridManager.Instance.GetClosestReachableNeighbour(CurrentNode, targetNode, pathfinder);
            }


            currentPath = pathfinder.FindPath(CurrentNode, targetNode);
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
                nextWaypoint = currentPath[pathIndex].WorldPosition;
                //Debug.Log($"Moving to Waypoint {pathIndex}: {nextWaypoint}");

                while (Vector3.Distance(transform.position, nextWaypoint) > 0.05f)
                {
                    transform.LookAt(nextWaypoint);
                    transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                if (pathIndex < currentPath.Count - 1)
                {
                    pathIndex++;
                    UpdateCurrentNode(currentPath[pathIndex]);
                    //Debug.Log(CurrentNode.GridPosition);
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

        public void SetSelected(bool selected)
        {
            Debug.Log("Updating unit material");
            if (renderers == null) Debug.Log("Renderers are null");

            Material mat = selected ? selectedMat : defaultMat;

            foreach (var rend in renderers)
            {
                if (rend.material != null)
                    rend.material = mat;
            }


        }
    }


}
