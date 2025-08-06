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

    private Building openBuilding;

    public bool Enabled;

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

        Enabled = true;
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

    // LISTEN FOR INPUT
    private void Update()
    {
        if (interactActions == null || Enabled == false)
        {
            HandleLeftRelease();
            return;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            HandleLeftRelease();
        }

        if (UnityEngine.EventSystems.EventSystem.current != null &&
        UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleLeftClick();
        }

        if (selectorBox.IsDragging)
        {
            selectorBox.UpdateDrag(interactActions.UI.Point.ReadValue<Vector2>());
        }
    }

    // ADD TO SELECTION

    // Starts drag
    private void HandleLeftClick()
    {
        ////Debug.Log("Left click detected!");
        selectorBox.BeginDrag(interactActions.UI.Point.ReadValue<Vector2>());
    }

    // On end drag, if it's too short to count as a drag, do single select, otherwise complete full drag and do multi select
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

    // On right click, if hit building, select building for deletion, if hit grid node, command units to grid node
    private void HandleRightClick(InputAction.CallbackContext ctx)
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GameObject hitObject = hit.collider.gameObject;
            Building building = hitObject.GetComponentInParent<Building>();

            if (building != null)
            {
                //Debug.Log("Hit a building component!");
                building.OpenMenu();
                openBuilding = building;
                return;
            }
        }
        if (openBuilding != null)
        {
            openBuilding.CloseMenu();
            openBuilding = null;
        }

        CommandUnits(interactActions.UI.Point.ReadValue<Vector2>());
    }
    // On left click, if hit building and is selected for deletion, delete, if hit unit add it to selection
    private void SingleClickSelect(Vector2 screenPos)
    {
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GameObject hitObject = hit.collider.gameObject;

            Building building = hitObject.GetComponentInParent<Building>();
            if (building != null)
            {
                ////Debug.Log("Hit a building component!");
                if (building.isSelected)
                {
                    openBuilding = null;
                    building.Delete();
                }
                else //Debug.Log("Building selected is not opn");
                    return;
            }

            ISelectableObject selectable = hitObject.GetComponentInParent<ISelectableObject>();
            if (selectable is Unit unit)
            {
                ClearSelection();

                AddToSelection(unit);
                //Debug.Log($"Adding {unit} to selection");
                return;
            }
        }
        else
        {
            if (openBuilding != null)
            {
                openBuilding.CloseMenu();
                openBuilding = null;
            }
        }
    }

    // add all units in drag box to selection list
    private void DragSelect(Vector2 start, Vector2 end)
    {
        if (cam == null ) return;

        Rect rect = selectorBox.GetScreenRect(start, end);
        ClearSelection();


        foreach (Unit unit in UnitManager.Instance.allUnits)
        {
            if(unit != null)
            {
                Vector3 screenPoint = cam.WorldToScreenPoint(unit.transform.position);
                if (screenPoint.z < 0) continue;

                Vector2 guiPoint = new(screenPoint.x, Screen.height - screenPoint.y);
                if (rect.Contains(guiPoint))
                {
                    AddToSelection(unit);
                }
            }
            
        }


    }

    // SELECTION
    private void AddToSelection(Unit unit)
    {
        ////Debug.Log($"Attempt add {unit} army {unit.Army} to selection");

        if (unit.Army != Army.Player)
        {
            //Debug.Log($"Attempt add {unit} army {unit.Army} to selection: Failed due to not player");
            return;
        }

        if (selectedUnits.Contains(unit))
        {
            //Debug.Log($"Attempt add {unit} army {unit.Army} to selection: Failed due to already selected");
            return;
        }
        

        // add color
        selectedUnits.Add(unit);

        FXManager.Instance.DoFX(FXType.Select);
        unit.GetComponent<Combatant>().SetSelected(true);

        //Debug.Log($"Attempt add {unit} army {unit.Army} to selection: Succeeded");
    }

    private void ClearSelection()
    {
        if(openBuilding != null) openBuilding.CloseMenu();
        openBuilding = null;

        if (selectedUnits.Count == 0) return;    


        foreach (Unit unit in selectedUnits)
        {
            if (unit != null)
            {
                unit.GetComponent<Combatant>().SetSelected(false);
            }
        }

        // change colors
        selectedUnits.Clear();
        //Debug.Log($"Cleared selection");
    }

    public bool UnitInSelection(Unit unitToCheck)
    {
        return selectedUnits.Contains(unitToCheck);
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
                //Debug.Log("SelectionManager: Target node is not walkable.");
                return;
            }

            FXManager.Instance.DoFX(FXType.BuildBuilding);
            SpawnRallyPoint(node);

            foreach (Unit unit in selectedUnits)
            {
                if (unit != null)
                {
                    unit.GetComponent<Combatant>().SetSelected(false);
                    if (unit is Combatant instance)
                        instance.SetTarget(node);

                    

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
