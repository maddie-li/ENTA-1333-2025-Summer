using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using RTS_1333;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public Building currentGhost;

    public BuildingType[] buildingTypes;

    public Material ValidMat;
    public Material InvalidMat;
    public Material EnemyMat;
    public Material PlayerMat;

    public Dictionary<Army, List<Building>> buildingsByArmy = new();

    public List<Building> allBuildings = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        buildingsByArmy.Add(Army.Player, new List<Building>());
        buildingsByArmy.Add(Army.Enemy, new List<Building>());
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
            NewGhost(buildingTypes[0]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            NewGhost(buildingTypes[1]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            NewGhost(buildingTypes[2]);*/

        if (currentGhost != null)
        {
            Placing();
        }
    }

    public void NewGhost(BuildingType typeToBuild)
    {
        if (currentGhost != null)
        {
            Destroy(currentGhost.gameObject);
        }

        Debug.Log(buildingTypes[0]);
        Debug.Log(typeToBuild.UnitPrefab);
        GameObject building = Instantiate(typeToBuild.UnitPrefab, this.transform);

        currentGhost = building.GetComponent<Building>();
        if (currentGhost != null)
        {
            currentGhost.isGhost = true;
            GridNode startNode = GridManager.Instance.GetNodeFromMousePosition();
            currentGhost.Initialize(startNode);
            currentGhost.SetNodePos(startNode);

            Material defaultMat = null;

            switch (currentGhost.army)
            {
                case Army.Player:
                    defaultMat = PlayerMat;
                    break;
                case Army.Enemy:
                    defaultMat = EnemyMat;
                    break;
            }

            if(defaultMat != null) currentGhost.SetupMat(defaultMat, ValidMat, InvalidMat);

            currentGhost.Spawner?.StopSpawning();
        }
    }

    private void Placing()
    {
        GridNode node = GridManager.Instance.GetNodeFromMousePosition();
        if (node == null) return;

        currentGhost.SetNodePos(node);

        bool validPlacement = !GridManager.Instance.IsFootprintOccupied(currentGhost.CurrentNode, currentGhost.Width, currentGhost.Length) && CurrencyManager.Instance.CanAfford(Army.Player, currentGhost);
        currentGhost.UpdateColor(validPlacement);

        // BUILD
        if (Mouse.current.leftButton.wasPressedThisFrame && validPlacement)
        {
            CurrencyManager.Instance.TryBuyUnit(Army.Player, currentGhost);
            FXManager.Instance.DoFX(FXType.BuildBuilding, GetFootprintCenter(currentGhost));
            Building();
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame && !validPlacement)
        {

            FXManager.Instance.DoFX(FXType.InvalidBuilding);
        }

        // CANCEL
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            FXManager.Instance.DoFX(FXType.Cancel);
            Destroy(currentGhost.gameObject);
            currentGhost = null;
        }
    }

    private void Building()
    {
        currentGhost.UpdateColor();
        currentGhost.isGhost = false;
        GridManager.Instance.FootprintOccupy(currentGhost.CurrentNode, currentGhost.Width, currentGhost.Length, currentGhost);
        RegisterUnit(currentGhost);
        currentGhost.Spawner?.StartSpawning();
        currentGhost = null;

    }

    public void EnemyBuilding(BuildingType typeToBuild, GridNode targetNode)
    {
        if (targetNode == null) return;
        if (!CurrencyManager.Instance.CanAfford(Army.Enemy, typeToBuild.Cost)) return;

        if (GridManager.Instance.IsFootprintOccupied(targetNode, typeToBuild.Width, typeToBuild.Length)) return;

        GameObject buildingObject = Instantiate(typeToBuild.UnitPrefab, this.transform);
        Building building = buildingObject.GetComponent<Building>();

        building.Initialize(targetNode);
        building.SetNodePos(targetNode);
        GridManager.Instance.FootprintOccupy(targetNode, building.Width, building.Length, building);
            
        RegisterUnit(building);
        building.Spawner?.StartSpawning();

        Material defaultMat = EnemyMat;
        building.SetupMat(defaultMat, ValidMat, InvalidMat);
        building.UpdateColor();

        FXManager.Instance.DoFX(FXType.BuildBuilding, GetFootprintCenter(building));

        CurrencyManager.Instance.TryBuyUnit(Army.Enemy, building);
    }

    public void RegisterUnit(Building building)
    {
        Debug.Log($"Registering building of army {building.army}");

        // add mainlist
        if (building != null && !allBuildings.Contains(building))
        {
            allBuildings.Add(building);
        }
        // add armylist
        if (!buildingsByArmy[building.army].Contains(building))
        {
            buildingsByArmy[building.army].Add(building);
        }
    }
    public void UnregisterUnit(Building building)
    {
        allBuildings.Remove(building);

        if (buildingsByArmy.TryGetValue(building.army, out var unitList))
        {
            unitList.Remove(building);
        }
    }

    public Vector3 GetFootprintCenter(Building building)
    {
        GridNode origin = building.CurrentNode;
        float nodeSize = GridManager.Instance.GridSettings.NodeSize; 

        Vector3 offset = new Vector3(
            (building.Width * nodeSize) / 2f,
            0f,
            (building.Length * nodeSize) / 2f
        );

        return origin.WorldPosition + offset;
    }

}
