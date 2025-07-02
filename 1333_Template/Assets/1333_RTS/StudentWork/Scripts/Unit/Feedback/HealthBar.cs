using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] Camera cam;
    void Update()
    {
        if (cam != null)
        {
            // billboard
            transform.forward = cam.transform.forward;
        }
    }
    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
}
