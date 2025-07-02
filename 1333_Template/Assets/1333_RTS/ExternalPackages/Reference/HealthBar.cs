using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public class HealthBar : MonoBehaviour
{
    public Vector3 Offset;
    private Controller controller;
    public Image image;
    
    public float Multiplier;

    void LateUpdate()
    {
        if(controller != null)
        {
            if(controller is NPC_Controller)
            {
                this.transform.position = controller.transform.position + Offset;
                transform.LookAt(Camera.main.transform);

                transform.localScale = Vector3.Distance(Camera.main.transform.position, transform.position) * (Vector3.one * Multiplier);
            }

            image.fillAmount = controller.CurrentHealth / controller.MaxHealth;

            if (controller.CurrentHealth <= 0)
            {
                Destroy(this.transform.gameObject);
            }
        }

        

    }

    public void LinkController(Controller controllerToLink)
    {
        controller = controllerToLink;
    }
}
*/