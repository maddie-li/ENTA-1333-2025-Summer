using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

public class BuildingPlacementUI : MonoBehaviour
{
    [SerializeField] private RectTransform layoutGroupParent;
    [SerializeField] private SelectBuildingButton buttonPrefab;

    public List<UnitData> buildings = new();
    public UnitManager buildingManager;

    void Start()
    {
        foreach(UnitData b in buildings)
        {
            SelectBuildingButton button = Instantiate(buttonPrefab, layoutGroupParent);
            button.Setup(b, buildingManager);
        }
    }

    
}
