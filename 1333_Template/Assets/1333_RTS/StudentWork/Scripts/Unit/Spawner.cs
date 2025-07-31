using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    public UnitData typeToSpawn;
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


    private void SpawnUnit()
    {
        Unit spawnedUnit;

        spawnedUnit = UnitManager.Instance.SpawnUnit(typeToSpawn.UnitPrefab, SpawnPoint.position, typeToSpawn.Army);

        if (spawnedUnit != null)
        {
            //Debug.LogWarning("Spawning Unit from building", spawnedUnit);
            spawnedUnit.movement.SetTarget(GridManager.Instance.GetNodeFromWorldPosition(RallyPoint.position));
        }
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
