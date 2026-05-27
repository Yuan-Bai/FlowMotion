
using UnityEngine;

public class PlayerSprintImpulseState : GroundedMoveState
{
    public PlayerSprintImpulseState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    #region 状态生命周期

    public override void Enter()
    {
        base.Enter();
        aniBridge.PlayClip("SprintImpulse", 0.2f, 0.1f);
    }

    public override void Update()
    {
        if (playerInput.dashHeldDuration>0.5f)
        {
            // 这里应该只能在sprint impulse状态进入，
            context.sprintEnabled = true;
        }

        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnAnimationEnterEvent()
    {
        base.OnAnimationEnterEvent();
        
    }

    public override void OnAnimationCompleteEvent()
    {
        base.OnAnimationExitEvent();
        if (context.sprintEnabled)
        {
            ChangeState(PlayerLocomotionStateId.Sprint);
        }
        else
        {
            if (context.runModeEnabled)
            {
                ChangeState(PlayerLocomotionStateId.Run);
            }
            else
            {
                ChangeState(PlayerLocomotionStateId.Walk);
            }
        }
    }
    #endregion
}