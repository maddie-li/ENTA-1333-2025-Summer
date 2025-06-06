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

    private BuildingData buildingData;

    public void Setup(BuildingData _buildingData)
    {
        buildingData = _buildingData;

        buttonText.text = buildingData.BuildingName;

    }

}
