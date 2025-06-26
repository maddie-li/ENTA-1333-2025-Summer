using System;
using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

public class Building : Unit
{
    private Renderer[] renderers;
    public bool isGhost = true;

    private Material validMat;
    private Material invalidMat;
    private Material defaultMat;

    public SpawnFromBuilding Spawner;

    private void Awake()
    {
        Spawner = GetComponent<SpawnFromBuilding>();
        
    }

    public void SetupMat(Material valid, Material invalid)
    {
        renderers = GetComponentsInChildren<Renderer>();
        defaultMat = GetComponentInChildren<Renderer>().material;

        invalidMat = invalid;
        validMat = valid;
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

        Material mat = isValid ? validMat : invalidMat;

        foreach (var rend in renderers)
        {

            if (rend.material != null)
                rend.material = mat;
        }
    }
    internal void UpdateColor()
    {
        foreach (var rend in renderers)
        {

            if (rend.material != null)
                rend.material = defaultMat;
        }
    }
}
