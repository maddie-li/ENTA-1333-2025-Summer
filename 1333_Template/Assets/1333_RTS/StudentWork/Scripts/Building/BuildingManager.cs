using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{

    [SerializeField] private GridManager gridManager;

    public List<BaseUnit> allBuildings = new();

    public BuildingInstance currentGhost;

    public BuildingType[] buildingTypes;

    public Material ValidMat;
    public Material InvalidMat;

    private void Update()
    {

        if (currentGhost != null)
        {
            Placing();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                NewGhost(buildingTypes[0]);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                NewGhost(buildingTypes[1]);
        }
    }

    public void NewGhost(BuildingType typeToBuild)
    {
        Debug.Log(buildingTypes[0]);
        Debug.Log(typeToBuild.UnitPrefab);
        GameObject building = Instantiate(typeToBuild.UnitPrefab);
        
        currentGhost = building.GetComponent<BuildingInstance>();
        if (currentGhost != null)
        {
            currentGhost.isGhost = true;
            GridNode startNode = gridManager.GetNodeFromMousePosition();
            currentGhost.Initialize(startNode);
            currentGhost.SetNodePos(startNode);
            currentGhost.SetupMat(ValidMat, InvalidMat);
        }
    }

    private void Placing()
    {
        var node = gridManager.GetNodeFromMousePosition();
        if (node == null) return;

        currentGhost.SetNodePos(node);

        bool validPlacement = !gridManager.IsFootprintOccupied(currentGhost.CurrentNode, currentGhost.Width, currentGhost.Length);
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
        gridManager.FootprintOccupy(currentGhost.CurrentNode, currentGhost.Width, currentGhost.Length, currentGhost);
        RegisterUnit(currentGhost);
        currentGhost = null;
    }

    public void RegisterUnit(BaseUnit unit)
    {
        if (unit != null && !allBuildings.Contains(unit))
        {
            allBuildings.Add(unit);
        }
    }
    public void UnregisterUnit(BaseUnit unit)
    {
        allBuildings.Remove(unit);
    }
}
