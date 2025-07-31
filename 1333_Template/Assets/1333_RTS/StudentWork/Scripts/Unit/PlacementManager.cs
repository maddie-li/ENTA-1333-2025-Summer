using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance;

    public Placement CurrentGhost;

    public UnitData[] UnitData;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (CurrentGhost != null)
        {
            Placing();
        }
    }

    public void NewGhost(UnitData typeToBuild)
    {
        if (CurrentGhost != null)
        {
            Destroy(CurrentGhost.gameObject);
        }

        Debug.Log(UnitData[0]);
        Debug.Log(typeToBuild.UnitPrefab);
        GameObject ghostObject = Instantiate(typeToBuild.UnitPrefab, this.transform);

        CurrentGhost = ghostObject.GetComponent<Placement>();
        if (CurrentGhost != null)
        {
            CurrentGhost.IsGhost = true;
            GridNode startNode = GridManager.Instance.GetNodeFromMousePosition();
            CurrentGhost.Unit.Initialize(startNode);
            CurrentGhost.Unit.SetNodePos(startNode);

            CurrentGhost.spawner?.StopSpawning();
        }
    }

    private void Placing()
    {
        GridNode node = GridManager.Instance.GetNodeFromMousePosition();
        if (node == null) return;

        CurrentGhost.Unit.SetNodePos(node);

        bool validPlacement = !GridManager.Instance.IsFootprintOccupied(CurrentGhost.Unit.CurrentNode, CurrentGhost.Unit.Width, CurrentGhost.Unit.Length)
            && CurrencyManager.Instance.CanAfford(Army.Player, CurrentGhost.Unit);
        CurrentGhost.UpdateColor(validPlacement);

        // BUILD
        if (Mouse.current.leftButton.wasPressedThisFrame && validPlacement)
        {
            CurrencyManager.Instance.TryBuyUnit(Army.Player, CurrentGhost.Unit);
            FXManager.Instance.DoFX(FXType.BuildBuilding, GetFootprintCenter(CurrentGhost.Unit));
            PlayerPlace();
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame && !validPlacement)
        {
            FXManager.Instance.DoFX(FXType.InvalidBuilding);
        }

        // CANCEL
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            FXManager.Instance.DoFX(FXType.Cancel);
            Destroy(CurrentGhost.gameObject);
            CurrentGhost = null;
        }
    }

    private void PlayerPlace()
    {
        CurrentGhost.UpdateColor();
        CurrentGhost.IsGhost = false;
        GridManager.Instance.FootprintOccupy(CurrentGhost.Unit.CurrentNode, CurrentGhost.Unit.Width, CurrentGhost.Unit.Length, CurrentGhost.Unit);
        UnitManager.Instance.RegisterUnit(CurrentGhost.Unit);
        CurrentGhost.spawner?.StartSpawning();
        CurrentGhost = null;
    }

    public void EnemyPlace(UnitData typeToBuild, GridNode targetNode)
    {
        if (targetNode == null) return;
        if (!CurrencyManager.Instance.CanAfford(Army.Enemy, typeToBuild.Cost)) return;
        if (GridManager.Instance.IsFootprintOccupied(targetNode, typeToBuild.Width, typeToBuild.Length)) return;

        GameObject ghostObjectObject = Instantiate(typeToBuild.UnitPrefab, this.transform);
        Unit ghostObject = ghostObjectObject.GetComponent<Unit>();

        ghostObject.Initialize(targetNode);
        ghostObject.SetNodePos(targetNode);
        GridManager.Instance.FootprintOccupy(targetNode, ghostObject.Width, ghostObject.Length, ghostObject);

        UnitManager.Instance.RegisterUnit(ghostObject);
        ghostObject.spawner?.StartSpawning();

        ghostObject.SetupMat();

        FXManager.Instance.DoFX(FXType.BuildBuilding, GetFootprintCenter(ghostObject));
        CurrencyManager.Instance.TryBuyUnit(Army.Enemy, ghostObject);
    }

    public Vector3 GetFootprintCenter(Unit ghostObject)
    {
        GridNode origin = ghostObject.CurrentNode;
        float nodeSize = GridManager.Instance.GridSettings.NodeSize;

        Vector3 offset = new Vector3(
            (ghostObject.Width * nodeSize) / 2f,
            0f,
            (ghostObject.Length * nodeSize) / 2f
        );

        return origin.WorldPosition + offset;
    }
}
