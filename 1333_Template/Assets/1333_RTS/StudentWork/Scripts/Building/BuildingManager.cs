using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{

    [SerializeField] private GridManager gridManager;

    public List<BaseUnit> allBuildings = new();

    public BuildingInstance currentGhost;

    public BuildingType buildingType;

    private void Update()
    {

        if (currentGhost != null)
        {
            Placing();
            
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
                NewGhost(buildingType);
        }
    }

    public void NewGhost(BuildingType typeToBuild)
    {
        GameObject building = Instantiate(typeToBuild.unitPrefab);
        
        currentGhost = building.GetComponent<BuildingInstance>();
        if (currentGhost != null)
        {
            currentGhost.isGhost = true;
            GridNode startNode = gridManager.GetNodeFromMousePosition();
            currentGhost.Initialize(startNode);
            currentGhost.SetNodePos(startNode);
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
            currentGhost.UpdateColor();
            currentGhost.isGhost = false; 
            gridManager.FootprintOccupy(currentGhost.CurrentNode, currentGhost.Width, currentGhost.Length, currentGhost);
            RegisterUnit(currentGhost);
            currentGhost = null;
        }
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
