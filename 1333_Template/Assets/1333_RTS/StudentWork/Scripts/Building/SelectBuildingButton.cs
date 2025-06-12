using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class SelectBuildingButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    private BuildingType buildingType;
    private BuildingManager buildingManager;

    public void Setup(BuildingType _buildingType, BuildingManager _buildingManager)
    {
        buildingType = _buildingType;

        buttonText.text = buildingType.name;

    }

    public void OnClick()
    {
        Debug.Log($"Selected building {buildingType}");
        buildingManager.NewGhost(buildingType);
    }

}
