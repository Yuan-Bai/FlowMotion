using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public sealed class PlayerInputReader : MonoBehaviour
{
    #region 输入
    private PlayerInputAction _inputAction;
    private InputAction _attackAction;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _moveSwitchAction;
    private InputAction _lookAction;
    private InputAction _zoomAction;
    private InputAction _dashAction;

    #endregion

    #region 供外部访问变量
    public Vector2 MoveAxis => _moveAction == null ? Vector2.zero : _moveAction.ReadValue<Vector2>();
    public Vector2 LookAxis => _lookAction == null ? Vector2.zero : _lookAction.ReadValue<Vector2>();
    public float ZoomDelta => _zoomAction == null ? 0.0f : _zoomAction.ReadValue<Vector2>().y;

    // 只在“按下这一刻”触发一次的逻辑
    public bool moveStartedThisFrame;
    public bool moveCancelThisFrame;
    public bool dashStartedThisFrame;
    public bool dashCancelThisFrame;
    public bool moveSwitchStartedThisFrame;

    public bool moveIsPressed;

    public float dashHeldDuration;
    #endregion

    #region 事件声明
    // private event UnityAction 
    #endregion

    #region Unity生命周期函数
    private void Awake()
    {
        _inputAction = new PlayerInputAction();

        var playerActions = _inputAction.Player;
        _attackAction = playerActions.Attack;
        _moveAction = playerActions.Move;
        _jumpAction = playerActions.Jump;
        _moveSwitchAction = playerActions.MoveSwitch;
        _lookAction = playerActions.Look;
        _zoomAction = playerActions.Zoom;
        _dashAction = playerActions.Dash;

        #region 事件注册
        _moveAction.started += OnMoveStarted;
        _moveAction.canceled += OnMoveCanceled;
        _dashAction.started += OnDashStarted;
        _dashAction.canceled += OnDashCanceled;
        _moveSwitchAction.started += OnMoveSwitchStarted;
        _moveSwitchAction.canceled += OnMoveSwitchCanceled;
        #endregion
    }

    private void OnEnable()
    {
        _inputAction?.Enable();
    }

    private void OnDisable()
    {
        _inputAction?.Disable();
        // 可能还需要处理一些变量的值，设置为默认状态

        ClearFrameInputs();
    }

    private void Update()
    {
        if (_dashAction.IsPressed())
        {
            dashHeldDuration += Time.deltaTime;
        }
        else
        {
            dashHeldDuration = 0;
        }
    }

    private void LateUpdate()
    {
        ClearFrameInputs();
    }

    private void OnDestroy()
    {
        #region 事件取消注册
        _moveAction.started -= OnMoveStarted;
        _moveAction.canceled -= OnMoveCanceled;
        _dashAction.started -= OnDashStarted;
        _dashAction.canceled -= OnDashCanceled;
        _moveSwitchAction.started -= OnMoveSwitchStarted;
        _moveSwitchAction.canceled -= OnMoveSwitchCanceled;
        #endregion


        _inputAction?.Dispose();
        _attackAction = null;
        _moveAction = null;
        _jumpAction = null;
        _moveSwitchAction = null;
        _lookAction = null;
        _zoomAction = null;
    }
    #endregion

    private void ClearFrameInputs()
    {
        moveStartedThisFrame = false;
        moveCancelThisFrame = false;
        dashStartedThisFrame = false;
        dashCancelThisFrame = false;
        moveSwitchStartedThisFrame = false;
    }

    #region 事件函数
    private void OnMoveStarted(InputAction.CallbackContext _)
    {
        moveStartedThisFrame = true;
        moveIsPressed = true;
    }
    private void OnMoveCanceled(InputAction.CallbackContext _)
    {
        moveCancelThisFrame = true;
        moveIsPressed = false;
    }
    private void OnDashStarted(InputAction.CallbackContext _)
    {
        dashStartedThisFrame = true;
    }
    private void OnDashCanceled(InputAction.CallbackContext _)
    {
        dashCancelThisFrame = true;
    }

    private void OnMoveSwitchStarted(InputAction.CallbackContext _)
    {
        moveSwitchStartedThisFrame = true;
    }
    private void OnMoveSwitchCanceled(InputAction.CallbackContext _)
    {
        
    }
    #endregion
}