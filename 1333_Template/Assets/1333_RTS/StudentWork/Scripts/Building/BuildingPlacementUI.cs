using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementUI : MonoBehaviour
{
    [SerializeField] private RectTransform layoutGroupParent;
    [SerializeField] private SelectBuildingButton buttonPrefab;
    [SerializeField] private BuildingType buildingData;

    void Start()
    {
        foreach(BuildingData b in buildingData.buildings)
        {
            SelectBuildingButton button = Instantiate(buttonPrefab, layoutGroupParent);
            button.Setup(b);
        }
    }
    
}
