using RTS_1333;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    void Update()
    {
        if (GridManager.Instance != null)
        {
            GridNode node = GridManager.Instance.GetNodeFromMousePosition();
            if (node != null)
            {
                transform.position = node.WorldPosition + new Vector3(0, 0.1f, 0);
            }
        }
    }
}
