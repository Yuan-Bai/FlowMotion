using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Data", fileName = "PlayerMoveStateConfig")]
public sealed class PlayerLocomotionConfig : ScriptableObject
{
    [field: SerializeField] public float Gravity {get; private set;} = -10;
    [field: SerializeField] public float MaxAngle {get; private set;} = 45f;
    [field: SerializeField] public PlayerIdleData PlayerIdleData { get; private set; } = new PlayerIdleData();
    [field: SerializeField] public PlayerMoveData PlayerWalkData { get; private set; } = new PlayerWalkData();
    [field: SerializeField] public PlayerMoveData PlayerRunData { get; private set; } = new PlayerRunData();
    [field: SerializeField] public PlayerMoveData PlayerSprintData { get; private set; } = new PlayerSprintData();
    [field: SerializeField] public PlayerMoveData PlayerSprintImpulseData { get; private set; } = new PlayerSprintData();
}


[Serializable]
public sealed class PlayerIdleData
{
    public PlayerLocomotionStateId stateId = PlayerLocomotionStateId.Idle;
    public float moveSpeed = 0;
    public float acceleration = 0;
    public float deceleration = 0;
    public float rotationSpeed = 5;
    public string animatorStateName = "";
    public Vector2 RandomIdleInterval = new(0, 1);
    public bool canJump;
    public bool canAttack;
    public bool canDash;
}

[Serializable]
public class PlayerMoveData
{
    public PlayerLocomotionStateId stateId;
    public float moveSpeed;
    public float acceleration;
    public float deceleration;
    public float rotationSpeed;
    public float inputThreshold;
    public float minSprintInput;
    public string enterTransitionName;
    public string exitTransitionName;
    public string animatorStateName;
    public bool canJump;
    public bool canAttack;
    public bool canDash;
}

[Serializable]
public sealed class PlayerWalkData : PlayerMoveData
{
}

[Serializable]
public sealed class PlayerRunData : PlayerMoveData
{
}

[Serializable]
public sealed class PlayerSprintData : PlayerMoveData
{
}