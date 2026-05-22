

using UnityEngine;

public class MovementState : IState<PlayerLocomotionStateId>
{
    public PlayerLocomotionStateId Id {get; private set; }
    protected PlayerContext context;
    protected PlayerLocomotionConfig config;
    
    #region context中的变量
    protected PlayerInputReader playerInput;
    protected CharacterController controller;
    #endregion

    public MovementState(PlayerLocomotionStateId id, PlayerContext context, PlayerLocomotionConfig config)
    {
        Id = id;
        this.context = context;
        this.config = config;

        playerInput = context.inputReader;
        controller = context.controller;
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

    public void OnAnimationTransitionEvent()
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

    public virtual void Update()
    {
    }

    protected void ChangeState(PlayerLocomotionStateId id)
    {
        context.stateMachine.ChangeState(id);
    }
}