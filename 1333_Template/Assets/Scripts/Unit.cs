using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name;
    public Vector3 WorldPosition;       // position in 3D space
    public Vector2Int GridPosition;     // grid coordinates
    public GridNode CurrentNode;

    public void Initialize(GridNode node)
    {
        Name = gameObject.name;
        GridPosition = node.GridPosition;
        WorldPosition = node.WorldPosition;
        CurrentNode = node;

        MoveTo(node);
    }

    public void MoveTo(GridNode newNode)
    {
        if(newNode.CurrentUnit != null)
        {
            Debug.Log("Move failed, node is occupied");
            return;
        }

        if (CurrentNode != null)
        {
            CurrentNode.CurrentUnit = null; // remove this from current node
        }

        CurrentNode = newNode;
        CurrentNode.CurrentUnit = this;

        // CHANGE POSITION
        gameObject.transform.position = WorldPosition;
    }
}
