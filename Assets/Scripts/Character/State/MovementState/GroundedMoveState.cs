
using UnityEngine;

public abstract class GroundedMoveState : MovementState
{
    private PlayerMoveData data;

    public GroundedMoveState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    #region 状态生命周期

    public override void Enter()
    {
        base.Enter();

        data = Id switch
        {
            PlayerLocomotionStateId.Walk => config.PlayerWalkData,
            PlayerLocomotionStateId.Run => config.PlayerRunData,
            PlayerLocomotionStateId.Sprint => config.PlayerSprintData,
            PlayerLocomotionStateId.Sprint_Impulse => config.PlayerSprintImpulseData,
            _ => config.PlayerWalkData
        };
        aniBridge.SetHasInput(true);
        aniBridge.SetMoveSpeed(data.moveSpeed);

        context.timeHub.Start("TurnbackCoolDown", 0.15f, true);
    }

    public override void Exit()
    {
        base.Exit();
        context.stopRequested = false;
        context.timeHub.Cancel("MoveToStopMove");
        context.canEnterStop = false;
    }

    public override void Update()
    {
        base.Update();
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
                ChangeState(PlayerLocomotionStateId.MoveStop);
                return;
            }
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
        if (playerInput.jumpStartedThisFrame )
        {
            ChangeState(PlayerLocomotionStateId.Jump);
            return;
        }
        UpdateMoveDirection(context.camTransform);
        if (Id != PlayerLocomotionStateId.Walk && Id != PlayerLocomotionStateId.Run &&
            context.timeHub.IsFinished("TurnbackCoolDown") &&
            Vector3.Dot(context.moveDirection, new Vector3(context.root.forward.x, 0.0f, context.root.forward.z)) < -0.7f
        )
        {
            ChangeState(PlayerLocomotionStateId.Turnback);
            context.sprintEnabled = true;
            return;
        }
        if (!context.stopRequested)
        {
            UpdateVelocity(data);
        }
    }

    #endregion

    private void UpdateMoveDirection(Transform camTransform)
    {
        Vector2 moveAxis = playerInput.MoveAxis;
        
        if (moveAxis == Vector2.zero)
        {
            // stopRequested为true，canEnterStop为false时进入，需要保持当前的速度
            return;
        }

        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward = new Vector3(forward.x, 0, forward.z);
        right = new Vector3(right.x, 0, right.z);

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveAxis.y + right * moveAxis.x;

        if (moveDirection.sqrMagnitude > 1)
            moveDirection.Normalize();

        context.moveDirection = moveDirection;
    }

    private void UpdateVelocity(PlayerMoveData data)
    {
        // 更新水平速度
        Vector3 targetHorizontalVelocity = data.moveSpeed * context.moveDirection;
        context.horizontalVelocity = Vector3.MoveTowards(
            context.horizontalVelocity,
            targetHorizontalVelocity,
            data.acceleration * Time.deltaTime
        );

        // 更新垂直速度

    }
}