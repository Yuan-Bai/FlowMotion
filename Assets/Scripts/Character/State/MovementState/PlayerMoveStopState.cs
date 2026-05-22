

using UnityEngine;

public class PlayerMoveStoppingState : MovementState
{
    public PlayerMoveStoppingState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    #region state周期
    public override void Enter()
    {
        base.Enter();
        
        // 不能设置movespeed，如果设置了会导致moveblend变为walk，如果此时是run，会导致walk->stoprun
        context.aniBridge.SetStoppingEnabled(true);
        context.stopRequested = false;
    }

    public override void Update()
    {
        // 后面会区分walk|run|sprint stop，目前暂时当作一类
        // 如果发现移动输入
        if (playerInput.moveIsPressed)
        {
            // 由ctrl切换walk和run，需要一个变量用于记录
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

        // 冲刺检查
        if (playerInput.dashStartedThisFrame)
        {
            ChangeState(PlayerLocomotionStateId.Dash);
            return;
        }

        // 之后做跳跃攻击等检测

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnAnimationEnterEvent()
    {
        base.OnAnimationEnterEvent();
        context.horizontalVelocity = Vector3.zero;
        context.verticalVelocity = 0.0f;
        context.aniBridge.SetMoveSpeed(0);
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
        // stop动画只播放一次，播放结束退回idle
        ChangeState(PlayerLocomotionStateId.Idle);
    }
    #endregion
}