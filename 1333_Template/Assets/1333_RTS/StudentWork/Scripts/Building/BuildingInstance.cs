using System;
using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

public class BuildingInstance : BaseUnit
{
    private Renderer[] renderers;
    public bool isGhost = true; 

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public override void UpdateCurrentNode(GridNode newNode)
    {
        if (isGhost)
        {
            CurrentNode = newNode;
        }
        else
        {
            
            if (newNode.CurrentUnit != null) return;

            if (CurrentNode != null)
                CurrentNode.CurrentUnit = null;

            CurrentNode = newNode;
            newNode.CurrentUnit = this;
        }
    }

    internal void UpdateColor(bool isValid)
    {
        Color color;


        if (isValid)
        {
            color = Color.green;
        }
        else
        {
            color = Color.red;
        }

        foreach (var rend in renderers)
        {
            
            if (rend.material != null)
                rend.material.color = color;
        }
    }
    internal void UpdateColor()
    {
        foreach (var rend in renderers)
        {

            if (rend.material != null)
                rend.material.color = Color.white;
        }
    }
}
