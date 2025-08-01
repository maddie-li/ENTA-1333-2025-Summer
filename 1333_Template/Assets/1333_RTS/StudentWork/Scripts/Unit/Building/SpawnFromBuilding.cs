using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SpawnType
{
    Units,
    Gold
}
public class SpawnFromBuilding : MonoBehaviour
{
    private Army army;
    [Header("Spawn Settings")]
    [SerializeField] public bool isSpawning = false;
    public SpawnType spawnType;

    public float SpawnInterval = 5f;

    [Header("Unit Spawn Settings")]

    public GameObject UnitPrefab;

    public Transform SpawnPoint;
    //public Transform RallyPoint;
    //public bool CanMoveRallyPoint = false;

    private float spawnTimer;

    [Header("Gold Spawn Settings")]
    [SerializeField] public int goldAmount = 1;
    void Update()
    {
        if (!isSpawning) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnInterval)
        {
            switch (spawnType)
            {
                case SpawnType.Units:
                    SpawnUnit();
                    break;
                case SpawnType.Gold:
                    SpawnGold();
                    break;
            }

            
            spawnTimer = 0f;
        }

        /*if (Mouse.current.leftButton.isPressed && CanMoveRallyPoint)
        {
            MoveRallyPoint();
        }*/
    }

    public void StartSpawning() => isSpawning = true;
    public void StopSpawning() => isSpawning = false;

    public void SetUnitPrefab(GameObject newPrefab) => UnitPrefab = newPrefab;

    private void SpawnUnit()
    {
        Combatant spawnedUnit;

        spawnedUnit = UnitManager.Instance.SpawnUnit(UnitPrefab, SpawnPoint.position, army);

        if (spawnedUnit != null)
        {
            //Debug.LogWarning("Spawning unit from building", spawnedUnit);
            //spawnedUnit.SetTarget(GridManager.Instance.GetNodeFromWorldPosition(RallyPoint.position));
            spawnedUnit.GoToClosestTarget();
        }
    }

    private void SpawnGold()
    {
        //Debug.Log($"Spawning gold for {army}");
        CurrencyManager.Instance.EarnGold(army, goldAmount);
    }

    public void SetArmy(Army _army)
    {
        army = _army;
    }

    /*private void MoveRallyPoint()
    {
        var node = GridManager.Instance.GetNodeFromMousePosition();
        if (node == null) return;

        bool validPlacement = !GridManager.Instance.IsFootprintOccupied(node);

        if (validPlacement)
        {
            RallyPoint.transform.position = node.WorldPosition;
        }
    }*/

}
