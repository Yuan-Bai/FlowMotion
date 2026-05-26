using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<TStateId> where TStateId : Enum
{
    public TStateId Id { get; }
    public void Enter();
    public void Exit();
    public void Update();
    public void PhysicsUpdate();
    public void HandleInput();
    public void OnTriggerEnter();
    public void OnTriggerExit();
    public void OnAnimationEnterEvent();
    public void OnAnimationExitEvent();
    public void OnAnimationCompleteEvent();

}
