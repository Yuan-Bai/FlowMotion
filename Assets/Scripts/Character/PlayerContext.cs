

using UnityEngine;

[System.Serializable]
public class PlayerContext
{
    #region 共享运行时引用
    public PlayerInputReader inputReader;
    public Transform root;
    public CharacterController controller;
    // public Animator animator;
    public Transform camTransform;
    public StateMachine<PlayerLocomotionStateId> stateMachine;
    public PlayerAnimationBridge aniBridge;
    public PlayerTimeHub timeHub;
    #endregion

    #region 共享运行时变量
    // 记录水平速度，用于player中controller控制
    public Vector3 horizontalVelocity;
    public float verticalVelocity;
    public Vector3 deltaPosition;
    public Quaternion deltaRotation;
    public Vector3 rootMotionVelocity;
    // 记录当前状态对应的速度
    // public float moveSpeed;
    // 记录当前状态对应的角色选择速度
    // public float rotationSpeed;
    public bool isGrounded;
    public Vector3 groundNormal;
    public Vector3 moveDirection;
    public bool isLockMove;
    public bool rootMotionPositionXZ;
    public bool rootMotionPositionY;
    public bool rootMotionRotation;
    public float stateTime;
    public float lastLandTime;
    public bool runModeEnabled;
    public bool sprintEnabled;
    public bool canEnterStop;
    public bool canEnterSprintImpulse;
    public bool canEnterMoveBlend;
    public bool stopRequested;
    public bool gravityEnabled = true;
    public bool canLand; // 防止离地马上进入fall
    public bool leftFootEnabled = true;
    public bool wasGroundedLastFrame = true;
    public bool justLeftGround;
    public bool justLanded;
    public float airborneMaxY;
    public float airborneStartY;
    public float landingY;
    public float fallDistance;
    public float heightFromGround;
    #endregion
}