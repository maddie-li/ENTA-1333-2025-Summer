using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }


    public Building currentGhost;

    public BuildingType[] buildingTypes;

    public Material ValidMat;
    public Material InvalidMat;

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
            NewGhost(buildingTypes[0]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            NewGhost(buildingTypes[1]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            NewGhost(buildingTypes[2]);

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
            currentGhost.SetupMat(ValidMat, InvalidMat);

            currentGhost.Spawner?.StopSpawning();
        }
    }

    private void Placing()
    {
        var node = GridManager.Instance.GetNodeFromMousePosition();
        if (node == null) return;

        currentGhost.SetNodePos(node);

        bool validPlacement = !GridManager.Instance.IsFootprintOccupied(currentGhost.CurrentNode, currentGhost.Width, currentGhost.Length);
        currentGhost.UpdateColor(validPlacement);

        // BUILD
        if (Mouse.current.leftButton.wasPressedThisFrame && validPlacement)
        {
            Building();
        }

        // CANCEL
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
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
}
