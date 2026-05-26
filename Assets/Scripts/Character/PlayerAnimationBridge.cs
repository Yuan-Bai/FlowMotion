using UnityEngine;

public class PlayerAnimationBridge : MonoBehaviour
{

    private Animator _animaotr;
    private StateMachine<PlayerLocomotionStateId> _locomotionStateMachine;
    private PlayerContext _context;

    public void Initialize(Animator animaotr, StateMachine<PlayerLocomotionStateId> locomotionStateMachine, PlayerContext context)
    {
        _animaotr = animaotr;
        _locomotionStateMachine = locomotionStateMachine;
        _context = context;
    }

    public void OnAnimatorMove()
    {
        _context.deltaPosition = _animaotr.deltaPosition;
        _context.deltaRotation = _animaotr.deltaRotation;
    }

    #region Animator参数设置
    public void SetMoveSpeed(float moveSpeed)
    {
        _animaotr.SetFloat(AnimatorID.MoveSpeedID, moveSpeed);
    }

    public void SetHasInput(bool hasInput)
    {
        _animaotr.SetBool(AnimatorID.HasInputID, hasInput);
    }
    #endregion

    #region 动画播放
    public void PlayDash()
    {
        _animaotr.CrossFadeInFixedTime("DashF", 0.1f);
    }

    public void PlayClip(string clipName, float transitionTime)
    {
        _animaotr.CrossFadeInFixedTime(clipName, transitionTime);
    }

    public void PlayClip(string clipName, float transitionTime, float offset)
    {
        _animaotr.CrossFadeInFixedTime(clipName, transitionTime, 0, offset);
    }
    #endregion

    #region 事件
    public void OnAnimationEnterEvent(PlayerLocomotionStateId stateId)
    {
        if (stateId == _locomotionStateMachine.CurrentStateId)
        {
            _locomotionStateMachine.OnAnimationEnterEvent();
        }
    }

    public void OnAnimationExitEvent(PlayerLocomotionStateId stateId)
    {
        if (stateId == _locomotionStateMachine.CurrentStateId)
        {
            _locomotionStateMachine.OnAnimationExitEvent();
        }
    }

    public void OnAnimationCompleteEvent(PlayerLocomotionStateId stateId)
    {
        if (stateId == _locomotionStateMachine.CurrentStateId)
        {
            _locomotionStateMachine.OnAnimationCompleteEvent();
        }
    }

    public void OnStopWindowEnterEvent(int footIndex)
    {
        // 没有进行stateid判断，可能会有问题
        // 窗口进入后，可能切换动画，需要在其他地方设置canEnterStop为false
        _context.canEnterStop = true;
        _animaotr.SetBool(AnimatorID.LeftFootEnabledID, footIndex==0);
    }

    public void OnStopWindowExitEvent()
    {
        _context.canEnterStop = false;
    }

    /// <summary>
    /// 用于dash动画clip过渡到sprintImpulse
    /// 没有设置退出窗口，通过state逻辑完成清除和先后判断
    /// </summary>
    public void OnSprintWindowEnterEvent(int stateId)
    {
        if (IsCurrentState(stateId))
        {
            _context.canEnterSprintImpulse = true;
        }
    }

    /// <summary>
    /// 用于dash动画clip过渡到moveBlend
    /// </summary>
    public void OnCanMoveWindowEnterEvent(int stateId)
    {
        if (IsCurrentState(stateId))
        {
            _context.canEnterMoveBlend = true;
        }
    }

    public void OnCanLandEvent(int stateId)
    {
        if (IsCurrentState(stateId))
        {
            // 变量与使用事件传递有何区别？
            _context.canLand = true;
        }
    }
    #endregion

    private bool IsCurrentState(int stateId)
    {
        return stateId == (int)_locomotionStateMachine.CurrentStateId;
    }
}