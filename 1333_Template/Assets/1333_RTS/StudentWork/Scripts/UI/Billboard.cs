using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float SizeMultiplier = 0.1f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        Vector3 dir = transform.position - mainCamera.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);

        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        Vector3 scaledSize = distance * (Vector3.one * SizeMultiplier);

        if (scaledSize.x < 1f)
        {
            transform.localScale = scaledSize;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }


}
