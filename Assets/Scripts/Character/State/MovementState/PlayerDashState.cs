
using UnityEngine;

public class PlayerDashState : MovementState
{
    public PlayerDashState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    public override void Enter()
    {
        base.Enter();
        aniBridge.SetHasInput(false);
        // context.aniBridge.SetMoveSpeed(0);
        if (context.inputReader.moveIsPressed)
        {
            aniBridge.PlayClip("DashF", 0.1f); 
        }
        else
        {
            aniBridge.PlayClip("DashB", 0.1f); 
        }

        context.horizontalVelocity = Vector3.zero;
        context.verticalVelocity = 0.0f;
        context.rootMotionPositionXZ = true;
    }

    public override void Exit()
    {
        base.Exit();
        context.canEnterSprintImpulse = false;
        context.canEnterMoveBlend = false;
        context.rootMotionPositionXZ = false;
        context.horizontalVelocity = new Vector3(context.rootMotionVelocity.x, 0, context.rootMotionVelocity.z);
        context.verticalVelocity = context.rootMotionVelocity.y;
    }

    public override void Update()
    {
        // 检查鼠标右键按下时间，判断是否进入sprint（只有在进入窗口才可以进入）
        // 如果按下后快速松开又按下，触发连续冲刺

        if (playerInput.moveIsPressed)
        {
            if (context.canEnterMoveBlend)
            {
                // 暂时不考虑进入sprint
                aniBridge.PlayClip("MoveBlend", 0.25f, 0.55f);
                if (context.runModeEnabled)
                {
                    ChangeState(PlayerLocomotionStateId.Run);
                }
                else
                {
                    ChangeState(PlayerLocomotionStateId.Walk);
                }
            }
            else if (context.canEnterSprintImpulse)
            {
                ChangeState(PlayerLocomotionStateId.Sprint_Impulse);
            }
        }
    }


    public override void OnAnimationEnterEvent()
    {
        base.OnAnimationEnterEvent();
    }

    public override void OnAnimationCompleteEvent()
    {
        base.OnAnimationCompleteEvent();
        ChangeState(PlayerLocomotionStateId.Idle);
    }

    // 切换后由于currentstate改变，这个函数可能永远不会被调用
    public override void OnAnimationExitEvent()
    {
    }
}