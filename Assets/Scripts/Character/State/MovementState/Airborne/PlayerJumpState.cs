


using UnityEngine;

public class PlayerJumpState : MovementState
{
    public PlayerJumpState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    public override void Enter()
    {
        base.Enter();
        aniBridge.PlayClip("JumpL", 0.1f);
        context.gravityEnabled = false;
        context.canLand = false;
        context.isRootMotion = true;
        context.verticalVelocity = 0.0f;
    }

    public override void Exit()
    {
        base.Exit();
        context.gravityEnabled = true;
        context.canLand = false;
        context.isRootMotion = false;
        if (context.rootMotionVelocity.y < 0)
        {
            context.verticalVelocity = context.rootMotionVelocity.y;
        }
        else
        {
            context.verticalVelocity = -2.0f;
            Debug.Log("采样不均导致");
        }
    }

    public override void Update()
    {
        base.Update();
        // 检测攻击
        // 检测滑翔
        // 可以被空中冲刺打断（二段跳）
        if (playerInput.dashStartedThisFrame)
        {
            ChangeState(PlayerLocomotionStateId.JumpSecond);
            return;
        }

        if (context.isGrounded && context.canLand)
        {
            ChangeState(PlayerLocomotionStateId.Land);
            return;
        }
    }

    public override void OnAnimationCompleteEvent()
    {
        base.OnAnimationCompleteEvent();
        // 进入Fall动画
        ChangeState(PlayerLocomotionStateId.Fall);
    }
}