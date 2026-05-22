
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : MovementState
{
    private bool canTriggerAction;
    private float waitDuration;
    private float waitTime;
    private int actionId;
    private IList<float> weight = new List<float>(){0.5f, 0.3f, 0.2f};

    public PlayerIdleState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config) : base(id, context, config)
    {
    }

    #region 动画状态周期函数
    public override void Enter()
    {
        base.Enter();
        context.horizontalVelocity = Vector3.zero;
        context.verticalVelocity = 0.0f;

        context.aniBridge.SetMoveSpeed(0); 

        actionId = 0;

        ResetWait();
    }

    public override void Update()
    {
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

        if (playerInput.dashStartedThisFrame)
        {
            ChangeState(PlayerLocomotionStateId.Dash);
            return;
        }

        if (actionId == 0)
        {
            if (!canTriggerAction)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= waitDuration)
                {
                    canTriggerAction = true;
                }
            }
            else
            {
                actionId = WeightedRandomizer();
                if (actionId == 0)
                {
                    ResetWait();
                }
                else
                {
                    context.aniBridge.SetIdleActionIndex(actionId);
                    context.aniBridge.SetIdleActionTrigger();
                }
            }
        }
    }

    public override void OnAnimationExitEvent()
    {
        base.OnAnimationExitEvent();
        actionId = 0;
        ResetWait();
    }
    #endregion

    private void ResetWait()
    {
        // 初始化随机等待变量
        canTriggerAction = false;
        waitTime = 0;
        waitDuration = Random.Range(4f, 8f);   
    }

    private int WeightedRandomizer()
    {
        float r = Random.Range(0f, 1f);
        float sum = 0f;
        for (int i=0;i<weight.Count;i++)
        {
            sum += weight[i];
            if (r <= sum) return i;
        }
        return weight.Count - 1;
    }
}