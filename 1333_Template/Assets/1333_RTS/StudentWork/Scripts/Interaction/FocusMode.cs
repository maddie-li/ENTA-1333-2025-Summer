using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

public class FocusMode : MonoBehaviour
{
    public static FocusMode Instance { get; private set; }
    public static bool IsFocusModeActive => Instance != null && Instance.isFocusActive;

    private InputSystem_Actions inputActions;
    private bool isFocusActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        if (inputActions == null) return;

        inputActions.Enable();
        inputActions.UI.FocusMode.performed += OnFocusPerformed;
        inputActions.UI.FocusMode.canceled += OnFocusCanceled;
    }

    private void OnDisable()
    {
        inputActions.UI.FocusMode.performed -= OnFocusPerformed;
        inputActions.UI.FocusMode.canceled -= OnFocusCanceled;
        inputActions.Disable();
    }

    private void OnFocusPerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Focusing");
        isFocusActive = true;
    }

    private void OnFocusCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Not focusing");
        isFocusActive = false;
    }
}
