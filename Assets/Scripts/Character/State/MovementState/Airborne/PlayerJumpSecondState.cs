

using UnityEngine;

public class PlayerJumpSecondState : MovementState
{
    public PlayerJumpSecondState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (context.inputReader.moveIsPressed)
        {
            aniBridge.PlayClip("JumpSecondF", 0.1f); 
        }
        else
        {
            aniBridge.PlayClip("JumpSecondB", 0.1f); 
        }
        context.gravityEnabled = false;
        context.canLand = false;
        context.rootMotionPositionY = true;
        context.verticalVelocity = 0.0f;
    }

    public override void Exit()
    {
        base.Exit();
        context.gravityEnabled = true;
        context.canLand = false;
        context.rootMotionPositionY = false;
        if (context.rootMotionVelocity.y < 0)
        {
            context.verticalVelocity = context.rootMotionVelocity.y;
        }
        else
        {
            context.verticalVelocity = -2.0f;
        }
        context.horizontalVelocity = new Vector3(context.rootMotionVelocity.x, 0, context.rootMotionVelocity.z);
    }

    public override void Update()
    {
        base.Update();
        // 检测攻击
        // 检测滑翔

        if (context.isGrounded && context.canLand)
        {
            ChangeState(PlayerLocomotionStateId.Land);
            return;
        }
    }

    public override void OnAnimationCompleteEvent()
    {
        base.OnAnimationCompleteEvent();

        // 进入下落循环
        ChangeState(PlayerLocomotionStateId.Fall);
    }
}