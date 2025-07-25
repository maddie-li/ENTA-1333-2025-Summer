using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnFromBuilding : MonoBehaviour
{

    public GameObject UnitPrefab;
    public float SpawnInterval = 5f;

    public Transform SpawnPoint;
    public Transform RallyPoint;
    public bool CanMoveRallyPoint = false;

    [SerializeField] public bool isSpawning = false;
    private float spawnTimer;

    void Update()
    {
        if (!isSpawning) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnInterval)
        {
            SpawnUnit();
            spawnTimer = 0f;
        }

        if (Mouse.current.leftButton.isPressed && CanMoveRallyPoint)
        {
            MoveRallyPoint();
        }
    }

    public void StartSpawning() => isSpawning = true;
    public void StopSpawning() => isSpawning = false;

    public void SetUnitPrefab(GameObject newPrefab) => UnitPrefab = newPrefab;

    private void SpawnUnit()
    {
        Combatant spawnedUnit;

        spawnedUnit = UnitManager.Instance.SpawnUnit(UnitPrefab, SpawnPoint.position);

        if (spawnedUnit != null)
        {
            Debug.LogWarning("Spawning unit from building", spawnedUnit);
            spawnedUnit.SetTarget(GridManager.Instance.GetNodeFromWorldPosition(RallyPoint.position));
        }
        /*else
        {
            Debug.LogError("Spawned unit inside building is null");
        }*/

    }

    private void MoveRallyPoint()
    {
        var node = GridManager.Instance.GetNodeFromMousePosition();
        if (node == null) return;

        bool validPlacement = !GridManager.Instance.IsFootprintOccupied(node);

        if (validPlacement)
        {
            RallyPoint.transform.position = node.WorldPosition;
        }
    }

}
