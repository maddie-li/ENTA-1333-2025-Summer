using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

    public void SetHealthColor(Color color)
    {
        if (fillImage != null)
            fillImage.color = color;
    }
}