using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementUI : MonoBehaviour
{
    [SerializeField] private RectTransform layoutGroupParent;
    [SerializeField] private SelectBuildingButton buttonPrefab;

    public List<BuildingType> buildings = new();
    public BuildingManager buildingManager;

    void Start()
    {
        foreach(BuildingType b in buildings)
        {
            SelectBuildingButton button = Instantiate(buttonPrefab, layoutGroupParent);
            button.Setup(b, buildingManager);
        }
    }

    
}
