using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    public static Selector Instance { get; set; }

    public Camera cam; 
    private SelectorBox selectorBox;

    private List<Unit> selectedUnits = new List<Unit>();

    private InputSystem_Actions interactActions;
    private Vector3 lastClickPosition;

    [SerializeField] private float minDragSize = 3f;
    [SerializeField] GameObject rallyPointPrefab;
    GameObject rallyPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        interactActions = new InputSystem_Actions();
        selectorBox = GetComponent<SelectorBox>();
        selectorBox.minDragSize = minDragSize;

        cam = Camera.main;
    }
    private void OnEnable()
    {
        if (interactActions == null) return;

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
        if (interactActions == null) return;

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
        // Debug.Log("Right click detected!");
        CommandUnits(interactActions.UI.Point.ReadValue<Vector2>());
    }

    private void SingleClickSelect(Vector2 screenPos)
    {
        ClearSelection();

        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            ISelectableObject selectable = hit.collider.GetComponentInParent<ISelectableObject>();
            if (selectable is Unit Unit)
            {
                ClearSelection();
                AddToSelection(Unit);
                Debug.Log($"Adding {Unit} to selection");
                return;
            }
        }

        ClearSelection();
    }

    private void DragSelect(Vector2 start, Vector2 end)
    {
        if (cam == null ) return;

        Rect rect = selectorBox.GetScreenRect(start, end);
        ClearSelection();


        foreach (Unit Unit in UnitManager.Instance.allUnits)
        {
            if(Unit != null)
            {
                Vector3 screenPoint = cam.WorldToScreenPoint(Unit.transform.position);
                if (screenPoint.z < 0) continue;

                Vector2 guiPoint = new(screenPoint.x, Screen.height - screenPoint.y);
                if (rect.Contains(guiPoint))
                    AddToSelection(Unit);
            }
            
        }

    }

    //SELECTION
    private void AddToSelection(Unit Unit)
    {
        //Debug.Log($"Attempt add {Unit} to selection");

        if (Unit.Army != Army.Player) return;

        if (selectedUnits.Contains(Unit)) return;

        // add color
        selectedUnits.Add(Unit);

        FXManager.Instance.DoFX(FXType.Select);
        Unit.GetComponent<Unit>().SetSelected(true);
    }

    private void ClearSelection()
    {
        if(selectedUnits.Count == 0) return;    

        FXManager.Instance.DoFX(FXType.Cancel);

        foreach (Unit Unit in selectedUnits)
        {
            if (Unit != null)
            {
                Unit.GetComponent<Unit>().SetSelected(false);
            }
        }

        // change colors
        selectedUnits.Clear();
        Debug.Log($"Cleared selection");
    }

    public bool UnitInSelection(Unit UnitToCheck)
    {
        return selectedUnits.Contains(UnitToCheck);
    }

    // COMMAND
    private void CommandUnits(Vector2 screenPos)
    {

        if (cam == null) return;
        if (selectedUnits.Count < 1) return;

        Ray ray = cam.ScreenPointToRay(screenPos); 
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 5f);
        Plane ground = new(Vector3.up, Vector3.zero);

        if (ground.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            lastClickPosition = ray.GetPoint(enter);

            GridNode node = GridManager.Instance.GetNodeFromWorldPosition(hitPoint);

            if (!node.Walkable)
            {
                Debug.Log("SelectionManager: Target node is not walkable.");
                return;
            }

            SpawnRallyPoint(node);

            foreach (Unit Unit in selectedUnits)
            {
                if (Unit != null)
                {
                    Unit.GetComponent<Unit>().SetSelected(false);
                    if (Unit is Unit instance)
                        instance.movement.SetTarget(node);

                    

                }
                
            }
        }
    }

    private void SpawnRallyPoint(GridNode node)
    {

        if (rallyPoint != null)
        {
            Destroy(rallyPoint);
        }

        rallyPoint = Instantiate(rallyPointPrefab);
        rallyPoint.transform.position = node.WorldPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; 
        Gizmos.DrawSphere(lastClickPosition, 0.2f); 
    }
}
