
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
        // 后面会区分walk|run|sprint|dash，目前暂时当作一类
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
            // sprint进入dash，应该保留进入sprint的flag，exit后sprintEnabled为false，故在此处重写置true
            if (Id == PlayerLocomotionStateId.Sprint)
            {
                context.sprintEnabled = true;
            }
            return;
        }

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
            context.aniBridge.SetModeOfBeforeStopMove(2);
            context.sprintEnabled = false;
        }
        else if (Id == PlayerLocomotionStateId.Walk)
        {
            context.aniBridge.SetModeOfBeforeStopMove(0);
        }
        else if (Id == PlayerLocomotionStateId.Run)
        {
            context.aniBridge.SetModeOfBeforeStopMove(1);
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