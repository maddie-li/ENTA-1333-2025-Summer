using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class Selector : MonoBehaviour
{
    public Camera cam; 
    private GridManager gridManager;
    private SelectorBox selectorBox;

    private List<BaseUnit> selectedUnits = new List<BaseUnit>();

    private InputSystem_Actions interactActions;
    private Vector3 lastClickPosition;

    [SerializeField] private float minDragSize = 3f;

    public void Initialize(GridManager _gridManager)
    {
        gridManager = _gridManager;
        interactActions = new InputSystem_Actions();
        selectorBox = GetComponent<SelectorBox>();
        selectorBox.minDragSize = minDragSize;
    }
    private void OnEnable()
    {
        interactActions.UI.RightClick.performed += HandleRightClick;
        interactActions.UI.Enable();
    }

    private void OnDisable()
    {
        interactActions.UI.RightClick.performed -= HandleRightClick;
        interactActions.UI.Disable();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleLeftClick();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            HandleLeftRelease();
        }

        if (selectorBox.IsDragging)
        {
            selectorBox.UpdateDrag(interactActions.UI.Point.ReadValue<Vector2>());
        }
    }

    private void HandleLeftClick()
    {
        Debug.Log("Left click detected!");
        selectorBox.BeginDrag(interactActions.UI.Point.ReadValue<Vector2>());
    }

    private void HandleLeftRelease()
    {
        if (selectorBox.IsDragging)
        {
            selectorBox.EndDrag(Mouse.current.position.ReadValue());

            if (selectorBox.DragDistance < minDragSize)
            {
                SingleClickSelect(selectorBox.DragEnd);
            }
            else
            {
                DragSelect(selectorBox.DragStart, selectorBox.DragEnd);
            }
        }

    }

    private void HandleRightClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("Right click detected!");
        CommandUnits(interactActions.UI.Point.ReadValue<Vector2>());
    }

    private void SingleClickSelect(Vector2 screenPos)
    {
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            ISelectableObject selectable = hit.collider.GetComponentInParent<ISelectableObject>();
            if (selectable is BaseUnit unit)
            {
                ClearSelection();
                AddToSelection(unit);
                Debug.Log($"Adding {unit} to selection");
                return;
            }
        }

        ClearSelection();
    }

    private void DragSelect(Vector2 start, Vector2 end)
    {
        if (cam == null || gridManager == null) return;

        Rect rect = selectorBox.GetScreenRect(start, end);
        ClearSelection();

        // REMOVE LATER 
        List<BaseUnit> units = new(FindObjectsByType<BaseUnit>(FindObjectsSortMode.None));

        foreach (BaseUnit unit in units)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(unit.transform.position);
            if (screenPoint.z < 0) continue;

            Vector2 guiPoint = new(screenPoint.x, Screen.height - screenPoint.y);
            if (rect.Contains(guiPoint))
                AddToSelection(unit);
        }
    }

    //SELECTION
    private void AddToSelection(BaseUnit unit)
    {
        if (selectedUnits.Contains(unit)) return;

        // add color
        selectedUnits.Add(unit);
    }

    private void ClearSelection()
    {
        // change colors
        selectedUnits.Clear();
        Debug.Log($"Cleared selection");
    }

    // COMMAND
    private void CommandUnits(Vector2 screenPos)
    {
        if (cam == null || gridManager == null) return;

        Ray ray = cam.ScreenPointToRay(screenPos); 
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 5f);
        Plane ground = new(Vector3.up, Vector3.zero);

        if (ground.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            lastClickPosition = ray.GetPoint(enter);

            GridNode node = gridManager.GetNodeFromWorldPosition(hitPoint);

            if (!node.Walkable)
            {
                Debug.Log("SelectionManager: Target node is not walkable.");
                return;
            }

            foreach (BaseUnit unit in selectedUnits)
            {
                if (unit is UnitInstance instance)
                    instance.SetTarget(node);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; 
        Gizmos.DrawSphere(lastClickPosition, 0.2f); 
    }
}
