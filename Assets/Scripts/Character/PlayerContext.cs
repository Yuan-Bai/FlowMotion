

using UnityEngine;

public class PlayerContext
{
    #region 共享运行时引用
    public PlayerInputReader inputReader;
    public Transform root;
    public CharacterController controller;
    public Animator animator;
    public Transform camTransform;
    public StateMachine<PlayerLocomotionStateId> stateMachine;
    public PlayerAnimationBridge aniBridge;
    #endregion

    #region 共享运行时变量
    // 记录水平速度，用于player中controller控制
    public Vector3 horizontalVelocity;
    public float verticalVelocity;
    // 记录当前状态对应的速度
    // public float moveSpeed;
    // 记录当前状态对应的角色选择速度
    // public float rotationSpeed;
    public bool isGrounded;
    public Vector3 groundNormal;
    public Vector3 moveDirection;
    public bool isLockMove;
    public bool isRootMotion;
    public float stateTime;
    public float lastLandTime;
    public bool runModeEnabled;
    public bool sprintEnabled;
    public bool canEnterStop;
    public bool canEnterSprintImpulse;
    public bool stopRequested;
    #endregion
}