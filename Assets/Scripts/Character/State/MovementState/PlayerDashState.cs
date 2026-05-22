
using UnityEngine;

public class PlayerDashState : MovementState
{
    public PlayerDashState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (context.inputReader.moveIsPressed)
        {
            context.aniBridge.SetDashFEnable(true);
            context.aniBridge.SetDashTrigger();
        }
        else
        {
            context.aniBridge.SetDashFEnable(false);
            context.aniBridge.SetDashTrigger();
        }

        context.canEnterSprintImpulse = false;
    }

    public override void Update()
    {
        // 检查鼠标右键按下时间，判断是否进入sprint（只有在进入窗口才可以进入）
        // 如果按下后快速松开又按下，触发连续冲刺

        if (context.canEnterSprintImpulse && playerInput.moveIsPressed)
        {
            ChangeState(PlayerLocomotionStateId.Sprint_Impulse);
        }
    }


    public override void OnAnimationEnterEvent()
    {
        base.OnAnimationEnterEvent();
        context.horizontalVelocity = Vector3.zero;
        context.verticalVelocity = 0.0f;
        // context.aniBridge.SetMoveSpeed(0);
    }

    public override void OnAnimationExitEvent()
    {
        // dash也是只播放一次，如果没有进入sprint或被其他状态打断就进入idle
        if (playerInput.moveIsPressed)
        {
            context.aniBridge.SetDashToMoveTrigger();
            if (context.runModeEnabled)
            {
                ChangeState(PlayerLocomotionStateId.Run);
            }
            else
            {
                ChangeState(PlayerLocomotionStateId.Walk);
            }
        }
        else
        {
            context.aniBridge.SetDashToIdleTrigger();
            ChangeState(PlayerLocomotionStateId.Idle);
        }
    }
}