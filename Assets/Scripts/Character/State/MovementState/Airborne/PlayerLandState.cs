

using UnityEngine;

public class PlayerLandState : MovementState
{
    public PlayerLandState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (context.fallDistance < 10f)
        {
            aniBridge.PlayClip("LandLight", 0.1f);
        }
        else
        {
            aniBridge.PlayClip("LandHeavy", 0.1f);
        }

        context.horizontalVelocity = Vector3.zero;
        // context.verticalVelocity = 0.0f;
    }

    public override void Update()
    {
        base.Update();
        // 重落下不可被打断
        // 轻落下可以被打断
        if (Id == PlayerLocomotionStateId.Land)
        {
            if (context.canEnterMoveBlend && playerInput.moveIsPressed)
            {
                aniBridge.PlayClip("MoveBlend", 0.25f, 0.55f);
                if (context.runModeEnabled)
                {
                    ChangeState(PlayerLocomotionStateId.Run);
                }
                else
                {
                    ChangeState(PlayerLocomotionStateId.Walk);
                }
                return;
            }

            if (playerInput.dashStartedThisFrame)
            {
                ChangeState(PlayerLocomotionStateId.Dash);
                return;
            }

            if (playerInput.jumpStartedThisFrame)
            {
                ChangeState(PlayerLocomotionStateId.Jump);
                return;
            }
        }
    }

    public override void OnAnimationCompleteEvent()
    {
        base.OnAnimationCompleteEvent();
        // 进入idle
        aniBridge.PlayClip("Idle", 0.25f);
        ChangeState(PlayerLocomotionStateId.Idle);
    }

    public override void Exit()
    {
        base.Exit();
        context.canEnterMoveBlend = false;
    }
}