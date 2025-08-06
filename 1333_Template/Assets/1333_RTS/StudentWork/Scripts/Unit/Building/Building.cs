using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using RTS_1333;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Building : Unit
{
    [Serializable]
    public class BuildingData
    {
        public float x;
        public float y;
        public float z;
    }


    private Renderer[] renderers;
    public bool isGhost = true;
    public bool isSelected = false;

    private Material validMat;
    private Material invalidMat;
    private Material defaultMat;

    public SpawnFromBuilding Spawner;

    [SerializeField] private List<BuildingData> data = new List<BuildingData>();  

    private void Awake()
    {
        Spawner = GetComponent<SpawnFromBuilding>();
        InitDamage();
    }
    private void Update()
    {
        if (!isGhost) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            transform.Rotate(Vector3.up, -90f, Space.World);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            transform.Rotate(Vector3.up, 90f, Space.World);
        }
    }

    public void SetupMat(Material regular, Material valid, Material invalid)
    {
        renderers = GetComponentsInChildren<Renderer>();
        //defaultMat = GetComponentInChildren<Renderer>().material;

        invalidMat = invalid;
        validMat = valid;
        defaultMat = regular;
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

    public void OpenMenu()
    {
        UpdateColor(false);
        ////Debug.Log("Setting open to true");
        isSelected = true;
    }

    public void CloseMenu()
    {
        UpdateColor();
        ////Debug.Log("setting open to false");
        isSelected = false;
    }

    public virtual void Delete()
    {
        if (!TryGetComponent<Unit>(out Unit thisUnit)) return;

        ////Debug.Log("Deregistering building");
        BuildingManager.Instance.UnregisterUnit(this);
        FXManager.Instance.DoFX(FXType.BuildingDestroy, this.WorldPosition);

        CurrencyManager.Instance.EarnGold(Army.Player, thisUnit.Cost / 2);

        Destroy(gameObject);

    }

}