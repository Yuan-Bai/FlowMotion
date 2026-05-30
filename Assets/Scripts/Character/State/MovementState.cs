

using UnityEngine;

public class MovementState : IState<PlayerLocomotionStateId>
{
    public PlayerLocomotionStateId Id {get; private set; }
    protected PlayerContext context;
    protected PlayerLocomotionConfig config;
    
    #region context中的变量
    protected PlayerInputReader playerInput;
    protected CharacterController controller;
    protected PlayerAnimationBridge aniBridge;
    #endregion

    public MovementState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config)
    {
        Id = id;
        this.context = context;
        this.config = config;

        playerInput = context.inputReader;
        controller = context.controller;
        aniBridge = context.aniBridge;
    }

    public virtual void Enter()
    {
        Debug.Log("进入" + Id);
    }

    public virtual void Exit()
    {
        Debug.Log("退出" + Id);
    }

    public void HandleInput()
    {
    }

    public virtual void OnAnimationEnterEvent()
    {
    }

    public virtual void OnAnimationExitEvent()
    {
    }

    public virtual void OnAnimationCompleteEvent()
    {
    }

    public void OnTriggerEnter()
    {
    }

    public void OnTriggerExit()
    {
    }

    public void PhysicsUpdate()
    {
    }
    
    private bool cdtStart;
    public virtual void Update()
    {
        if (!context.isGrounded
        && Id != PlayerLocomotionStateId.Jump
        && Id != PlayerLocomotionStateId.JumpSecond
        && Id != PlayerLocomotionStateId.Fall
        && !cdtStart)
        {
            context.timeHub.Start("WaitToFallFor", 0.1f, true, PlayerLocomotionStateId.Null, () => {ChangeState(PlayerLocomotionStateId.Fall);});
            cdtStart = true;
        }
        
        if (!context.isGrounded
        && Id != PlayerLocomotionStateId.Jump
        && Id != PlayerLocomotionStateId.JumpSecond
        && Id != PlayerLocomotionStateId.Fall
        )
        {
            context.timeHub.Cancel("WaitToFallFor");
            cdtStart = false;
        }
    }

    protected void ChangeState(PlayerLocomotionStateId id)
    {
        context.stateMachine.ChangeState(id);
    }
}