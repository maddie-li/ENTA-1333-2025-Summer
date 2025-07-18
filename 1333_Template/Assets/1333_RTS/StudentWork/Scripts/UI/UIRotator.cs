using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotator : MonoBehaviour
{
    public float speed;
    private void Update()
    {

        transform.eulerAngles += new Vector3(0,0, Time.deltaTime *  speed);
    }
}
