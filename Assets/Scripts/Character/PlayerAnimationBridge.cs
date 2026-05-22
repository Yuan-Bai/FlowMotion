using System.Collections;
using UnityEngine;

public class PlayerAnimationBridge : MonoBehaviour
{
    // 组件获取
    private Animator _animator;

    // player传递参数获取
    private StateMachine<PlayerLocomotionStateId> _stateMachine;
    private PlayerContext _context;

    // 基本参数
    private float currentMoveSpeed;
    private Coroutine myCoroutine = null;

    private int _moveSpeed = Animator.StringToHash("MoveSpeed");
    private int _modeOfBeforeStopMove = Animator.StringToHash("ModeOfBeforeStopMove");
    private int _stoppingEnabled = Animator.StringToHash("StoppingEnabled");
    private int _leftFootEnabled = Animator.StringToHash("LeftFootEnabled");
    private int _idleActionIndex = Animator.StringToHash("IdleActionIndex");
    private int _idleActionTrigger = Animator.StringToHash("IdleActionTrigger");
    private int _dashTrigger = Animator.StringToHash("DashTrigger");
    private int _dashFEnable = Animator.StringToHash("DashFEnable");
    private int _sprintImpulseToMoveBlendEnabled = Animator.StringToHash("SprintImpulseToMoveBlendEnabled");
    private int _sprintImpulseTrigger = Animator.StringToHash("SprintImpulseTrigger");
    private int _dashToIdleTrigger = Animator.StringToHash("DashToIdleTrigger");
    private int _dashToMoveTrigger = Animator.StringToHash("DashToMoveTrigger");

    public void Initialize(PlayerContext context)
    {
        _context = context;
        _stateMachine = context.stateMachine;
        _animator = GetComponent<Animator>();

    }

    private IEnumerator ToTargetSpeed(float targetSpeed)
    {
        while (currentMoveSpeed != targetSpeed)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetSpeed, 5f * Time.deltaTime);
            _animator.SetFloat(_moveSpeed, currentMoveSpeed);
            yield return null;
        }
        myCoroutine = null;
    }

    #region 动画参数设置
    public void SetMoveSpeed(float moveSpeed)
    {
        if (myCoroutine != null) StopCoroutine(myCoroutine);
        currentMoveSpeed = moveSpeed;
        _animator.SetFloat(_moveSpeed, moveSpeed);
    }

    public void SetMoveSpeedToTarget(float targetSpeed)
    {
        if (myCoroutine != null) StopCoroutine(myCoroutine);
        myCoroutine = StartCoroutine(ToTargetSpeed(targetSpeed));
    }

    public void SetModeOfBeforeStopMove(int modeOfBeforeStopMove)
    {
        _animator.SetInteger(_modeOfBeforeStopMove, modeOfBeforeStopMove);
    }

    public void SetStoppingEnabled(bool stoppingEnabled)
    {
        _animator.SetBool(_stoppingEnabled, stoppingEnabled);
    }

    public void SetIdleActionIndex(int idleActionIndex)
    {
        _animator.SetInteger(_idleActionIndex, idleActionIndex);
    }

    public void SetIdleActionTrigger()
    {
        _animator.SetTrigger(_idleActionTrigger);
    }

    public void SetDashTrigger()
    {
        _animator.SetTrigger(_dashTrigger);
    }

    public void SetDashFEnable(bool dashFEnable)
    {
        _animator.SetBool(_dashFEnable, dashFEnable);
    }

    public void SetSprintImpulseToMoveBlendEnabled(bool sprintImpulseToMoveBlendEnabled)
    {
        _animator.SetBool(_sprintImpulseToMoveBlendEnabled, sprintImpulseToMoveBlendEnabled);
    }

    public void SetSprintImpulseTrigger()
    {
        _animator.SetTrigger(_sprintImpulseTrigger);
    }

    public void SetDashToIdleTrigger()
    {
        _animator.SetTrigger(_dashToIdleTrigger);
    }

    public void SetDashToMoveTrigger()
    {
        _animator.SetTrigger(_dashToMoveTrigger);
    }
   #endregion

    #region 动画事件
    public void OnAnimationEnterEvent(int stateId)
    {
        _stateMachine.OnAnimationEnterEvent();  
    }

    public void OnAnimationExitEvent()
    {
        _stateMachine.OnAnimationExitEvent();
    }

    public void OnStopWindowEnter(int leftFootEnabled)
    {
        _animator.SetBool(_leftFootEnabled, leftFootEnabled==0);
        _context.canEnterStop = true;
    }

    public void OnStopWindowExit()
    {
        _context.canEnterStop = false;
    }

    public void OnSprintWindowEnter()
    {
        _context.canEnterSprintImpulse = true;
    }

    public void OnSprintWindowExit()
    {
        _context.canEnterSprintImpulse = false;
    }
    #endregion

}