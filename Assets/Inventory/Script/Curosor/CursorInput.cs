using UnityEngine;
using UnityEngine.InputSystem;

public class CursorInput : MonoBehaviour
{
    [SerializeField] private Cursor cursor;

    [Header("Left Click Actions")]
    [SerializeField] private InputActionReference leftSingleClick;
    [SerializeField] private InputActionReference leftDoubleClick;
    [SerializeField] private InputActionReference leftHold;
    [SerializeField] private InputActionReference leftShiftSingleClick;

    [Header("Right Click Actions")]
    [SerializeField] private InputActionReference rightSingleClick;
    [SerializeField] private InputActionReference rightHold;

    private const float DoubleClickTime = 0.3f;

    private bool _isLeftHolding;
    private bool _isRightHolding;
    private float _timer;

    // ----- Life Cycle Methods -----

    private void OnEnable()
    {
        leftSingleClick.action.Enable();
        leftDoubleClick.action.Enable();
        leftHold.action.Enable();
        leftShiftSingleClick.action.Enable();

        rightSingleClick.action.Enable();
        rightHold.action.Enable();

        leftSingleClick.action.performed += OnLeftSingleClick;
        leftDoubleClick.action.performed += OnLeftDoubleClick;
        leftHold.action.performed += OnLeftHoldPerformed;
        leftHold.action.canceled += OnLeftHoldCanceled;
        leftShiftSingleClick.action.performed += OnLeftShiftSingleClick;

        rightSingleClick.action.performed += OnRightSingleClick;
        rightHold.action.performed += OnRightHoldPerformed;
        rightHold.action.canceled += OnRightHoldCanceled;
    }

    private void OnDisable()
    {
        leftSingleClick.action.performed -= OnLeftSingleClick;
        leftDoubleClick.action.performed -= OnLeftDoubleClick;
        leftHold.action.performed -= OnLeftHoldPerformed;
        leftHold.action.canceled -= OnLeftHoldCanceled;
        leftShiftSingleClick.action.performed -= OnLeftShiftSingleClick;

        rightSingleClick.action.performed -= OnRightSingleClick;
        rightHold.action.performed -= OnRightHoldPerformed;
        rightHold.action.canceled -= OnRightHoldCanceled;

        leftSingleClick.action.Disable();
        leftDoubleClick.action.Disable();
        leftHold.action.Disable();
        leftShiftSingleClick.action.Disable();

        rightSingleClick.action.Disable();
        rightHold.action.Disable();
    }

    private void Update()
    {
        if (_isLeftHolding)
        {
            cursor.OnLeftHolding();
        }

        if (_isRightHolding)
        {
            cursor.OnRightHolding();
        }

        if (_timer < DoubleClickTime)
        {
            _timer += Time.deltaTime;
        }
    }

    // ----- Input Action Methods -----

    private void OnLeftSingleClick(InputAction.CallbackContext ctx)
    {
        if (Keyboard.current.shiftKey.isPressed) return; // シフト左クリックと区別する

        if (_timer < DoubleClickTime) return;

        cursor.OnLeftSingleClick();
        _timer = 0;
    }

    private void OnLeftDoubleClick(InputAction.CallbackContext ctx)
    {
        cursor.OnLeftDoubleClick();
    }

    private void OnLeftHoldPerformed(InputAction.CallbackContext ctx)
    {
        _isLeftHolding = true;
        cursor.OnLeftHoldInit();
    }
    private void OnLeftHoldCanceled(InputAction.CallbackContext ctx)
    {
        _isLeftHolding = false;
    }

    private void OnLeftShiftSingleClick(InputAction.CallbackContext ctx)
    {
        cursor.OnLeftShiftSingleClick();
    }

    private void OnRightSingleClick(InputAction.CallbackContext ctx)
    {
        cursor.OnRightSingleClick();
    }

    private void OnRightHoldPerformed(InputAction.CallbackContext ctx)
    {
        _isRightHolding = true;
        cursor.OnRightHoldInit();
    }
    private void OnRightHoldCanceled(InputAction.CallbackContext ctx)
    {
        _isRightHolding = false;
    }
}

