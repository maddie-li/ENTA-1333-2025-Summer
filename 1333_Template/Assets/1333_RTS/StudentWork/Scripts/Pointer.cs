using RTS_1333;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    void Update()
    {
        GridNode node = GridManager.Instance.GetNodeFromMousePosition();
        transform.position = node.WorldPosition + new Vector3(0,0.1f,0);

    }
}
