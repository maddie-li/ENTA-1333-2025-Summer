using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectBuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    private BuildingType buildingType;

    public void Setup(BuildingType _buildingType, BuildingManager _buildingManager)
    {
        buildingType = _buildingType;

        buttonText.text = buildingType.name;

        button.onClick.AddListener(() =>
        {
            Debug.Log($"Selected building {buildingType} add listener");
            BuildingManager.Instance.NewGhost(buildingType);
        });


    }

    public void OnClick()
    {
        Debug.Log($"Selected building {buildingType} via onclick");
        FXManager.Instance.DoFX(FXType.Select);
        BuildingManager.Instance.NewGhost(buildingType);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.text = $"{buildingType.Cost} Gold";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.text = buildingType.name;
    }

}
