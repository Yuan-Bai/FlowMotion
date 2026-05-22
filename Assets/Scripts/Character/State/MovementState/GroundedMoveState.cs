
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

        context.aniBridge.SetMoveSpeedToTarget(data.moveSpeed);
        context.aniBridge.SetStoppingEnabled(false);
    }

    public override void Update()
    {
        if (!context.stopRequested)
        {
            UpdateMoveDirection(context.camTransform);
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