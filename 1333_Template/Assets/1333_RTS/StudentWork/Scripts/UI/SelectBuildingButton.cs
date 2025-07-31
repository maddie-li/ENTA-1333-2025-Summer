using RTS_1333;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectBuildingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Button button;

    private UnitData buildingType;

    public void Setup(UnitData _buildingType)
    {
        buildingType = _buildingType;

        buttonText.text = buildingType.name;

        button.onClick.AddListener(() =>
        {
            Debug.Log($"Selected building {buildingType} add listener");
            PlacementManager.Instance.NewGhost(buildingType);
        });


    }

    public void OnClick()
    {
        Debug.Log($"Selected building {buildingType} via onclick");
        FXManager.Instance.DoFX(FXType.Select);
        PlacementManager.Instance.NewGhost(buildingType);
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
