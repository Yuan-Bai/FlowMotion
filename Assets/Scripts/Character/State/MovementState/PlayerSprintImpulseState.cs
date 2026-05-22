
using UnityEngine;

public class PlayerSprintImpulseState : GroundedMoveState
{
    private int loop_count = 2;
    private int current_loop_count;
    public PlayerSprintImpulseState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    #region 状态生命周期

    public override void Enter()
    {
        base.Enter();
        context.aniBridge.SetSprintImpulseToMoveBlendEnabled(false);
        context.aniBridge.SetSprintImpulseTrigger();
        current_loop_count = 1;
    }

    public override void Update()
    {
        // 如果取消移动输入
        if (!playerInput.moveIsPressed)
        {
            context.stopRequested = true;
            if (context.canEnterStop)
            {
                ChangeState(PlayerLocomotionStateId.MoveStop);
                return;
            }
        }
        else
        {
            context.stopRequested = false;
        }

        if (playerInput.dashStartedThisFrame)
        {
            ChangeState(PlayerLocomotionStateId.Dash);
            return;
        }

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

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
        if (current_loop_count != loop_count)
        {
            current_loop_count += 1;
            return;
        }
        context.aniBridge.SetSprintImpulseToMoveBlendEnabled(true);
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