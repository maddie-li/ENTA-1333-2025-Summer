using RTS_1333;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float interval = 10f;
    [SerializeField] private int spawnDistance = 5;
    [SerializeField] private BuildingType testType;

    private float timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            DoAction();
            timer = 0f;
        }
    }

    private void DoAction()
    {
        List<System.Action> actions = new List<System.Action>
        {
             TryBuild,
             //TryMoveTroops
        };

        System.Action selected = PickRandom(actions);
        selected?.Invoke();
    }

    private void TryBuild()
    {
        GridNode nodeToBuildOn = null;
        List<Building> existingBuildings = BuildingManager.Instance.buildingsByArmy[Army.Enemy];

        if (existingBuildings.Count == 0)
        {
            nodeToBuildOn = GridManager.Instance.GetRandomFreeNode();
        }
        else
        {
            Building lastBuilding = existingBuildings[existingBuildings.Count - 1];
            List<GridNode> nearbyNodes = GridManager.Instance.GetNodesAtDistance(lastBuilding.CurrentNode, spawnDistance);
            List<GridNode> spawnableNodes = new List<GridNode>();

            foreach (GridNode node in nearbyNodes)
            {
                if (node.Walkable && !node.IsOccupied()) spawnableNodes.Add(node);
            }

            nodeToBuildOn = PickRandom(spawnableNodes);
        }

        BuildingManager.Instance.EnemyBuilding(testType, nodeToBuildOn);
    }

    private void TryMoveTroops()
    {
        Debug.Log("AI: Moving troops...");
        // Add unit movement logic here
    }

    private T PickRandom<T>(List<T> list)
    {
        return list.Count > 0 ? list[Random.Range(0, list.Count)] : default;
    }

}
