
using UnityEngine;

public class PlayerMoveState : GroundedMoveState
{
    public PlayerMoveState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    #region 状态生命周期

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (context.sprintEnabled)
        {
            // sprint状态

        }
        else
        {
            // 非sprint状态
            // run与walk之间的切换
            if (context.runModeEnabled&&Id!=PlayerLocomotionStateId.Run)
            {
                ChangeState(PlayerLocomotionStateId.Run);
                return;
            }
            else if (!context.runModeEnabled&&Id!=PlayerLocomotionStateId.Walk)
            {
                ChangeState(PlayerLocomotionStateId.Walk);
                return;
            }
        }
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        if (Id == PlayerLocomotionStateId.Sprint)
        {
            context.sprintEnabled = false;
        }
        else if (Id == PlayerLocomotionStateId.Walk)
        {
        }
        else if (Id == PlayerLocomotionStateId.Run)
        {
        }
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
        // 目前只为SprintImpulse添加了动画结束事件，但也添加一个判断
        if (Id == PlayerLocomotionStateId.Sprint_Impulse)
        {
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
    }
    #endregion
}