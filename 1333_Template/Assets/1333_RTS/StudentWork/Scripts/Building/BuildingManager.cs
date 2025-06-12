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
            currentGhost.SetNodePos(gridManager.GetNodeFromMousePosition());
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
            RegisterUnit(currentGhost);
            Debug.Log(gridManager.GetNodeFromMousePosition());
            currentGhost.Initialize(gridManager.GetNodeFromMousePosition());
            currentGhost.SetNodePos(gridManager.GetNodeFromMousePosition());
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
