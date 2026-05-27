

using UnityEngine;

public class PlayerTurnbackState : MovementState
{
    public PlayerTurnbackState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    public override void Enter()
    {
        base.Enter();
        aniBridge.PlayClip("RunTurnback", 0.1f);
        context.rootMotionPositionXZ = true;
        context.rootMotionRotation = true;
    }

    public override void Exit()
    {
        base.Exit();
        context.rootMotionPositionXZ = false;
        context.rootMotionRotation = false;
        context.stopRequested = false;
        context.timeHub.Cancel("MoveToStopMove");
        context.horizontalVelocity = new Vector3(context.rootMotionVelocity.x, 0, context.rootMotionVelocity.z);
        context.verticalVelocity = context.rootMotionVelocity.y;
        context.canEnterStop = false;
    }

    public override void Update()
    {
        base.Update();
        // 可以被攻击，跳跃，冲刺，movestop打断
        if (playerInput.jumpStartedThisFrame)
        {
            ChangeState(PlayerLocomotionStateId.Jump);
            return;
        }

        if (playerInput.dashStartedThisFrame)
        {
            ChangeState(PlayerLocomotionStateId.Dash);
            return;
        }

        // 如果取消移动输入
        if (!playerInput.moveIsPressed && !context.stopRequested)
        {
            context.timeHub.Start("MoveToStopMove", 0.15f, true);
            context.stopRequested = true;
        }
        if (playerInput.moveIsPressed)
        {
            context.timeHub.Cancel("MoveToStopMove");
            context.stopRequested = false;
        }
        if (context.stopRequested && context.timeHub.IsFinished("MoveToStopMove"))
        {
            if (context.canEnterStop)
            {
                if (context.leftFootEnabled)
                {
                    aniBridge.PlayClip("StopSprintL", 0.25f);
                }
                else
                {
                    aniBridge.PlayClip("StopSprintR", 0.25f);
                }
                ChangeState(PlayerLocomotionStateId.MoveStop);
                return;
            }
        }
    }

    public override void OnAnimationCompleteEvent()
    {
        base.OnAnimationCompleteEvent();
        aniBridge.PlayClip("MoveBlend", 0.25f, 0.3f);
        ChangeState(PlayerLocomotionStateId.Sprint);
    }
}