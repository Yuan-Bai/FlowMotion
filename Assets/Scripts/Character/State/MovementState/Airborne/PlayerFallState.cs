

public class PlayerFallState : MovementState
{
    public PlayerFallState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    public override void Enter()
    {
        base.Enter();
        aniBridge.PlayClip("Fall", 0.1f);
    }

    public override void Update()
    {
        base.Update();
        // 检测攻击
        // 检测滑翔

        // 检测是否落地
        if (context.isGrounded)
        {
            ChangeState(PlayerLocomotionStateId.Land);
            return;
        }

        if (playerInput.dashStartedThisFrame)
        {
            ChangeState(PlayerLocomotionStateId.JumpSecond);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}