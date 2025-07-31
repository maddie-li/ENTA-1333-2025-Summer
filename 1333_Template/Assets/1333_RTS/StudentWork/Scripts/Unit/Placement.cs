using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using RTS_1333;
using Unity.Mathematics;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public Unit Unit;
    private UnitData UnitData;

    private Renderer[] renderers;
    public bool IsGhost = true;
    public Spawner spawner;
    private Material validMat;
    private Material invalidMat;
    private Material defaultMat;

    [Header("Placement Settings")]

    [SerializeField] private int value = 1;
    [SerializeField] private int buildTime = 1;
    public int Value => value;
    public int BuildTime => buildTime;

    private void Awake()
    {
        Unit = GetComponentInParent<Unit>();
        UnitData = Unit.UnitData;

        spawner = Unit.spawner;
    }

    public void SetupMat(Material regular, Material valid, Material invalid)
    {
        renderers = GetComponentsInChildren<Renderer>();

        invalidMat = invalid;
        validMat = valid;
        defaultMat = regular;
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