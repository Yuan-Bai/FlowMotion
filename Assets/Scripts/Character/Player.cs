using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(PlayerMotor))]

[RequireComponent(typeof(PlayerAnimationBridge))]

public class Player : MonoBehaviour
{
    #region 人物状态参数
    [SerializeField] private PlayerLocomotionConfig config;
    #endregion

    #region 状态变量
    private StateMachine<PlayerLocomotionStateId> stateMachine;
    private PlayerContext _context;
    #endregion

    #region Motor
    private GroundDetector _groundDetector;
    private PlayerMotor _motor;
    #endregion

    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;

    private void Awake()
    {
        _groundDetector = GetComponent<GroundDetector>();
        _motor = GetComponent<PlayerMotor>();

        stateMachine = new();
        _context = new();
        _context.inputReader = GetComponent<PlayerInputReader>();
        _context.root = transform;
        _context.controller = GetComponent<CharacterController>();
        _context.animator = GetComponent<Animator>();
        _context.camTransform = Camera.main.transform;
        _context.stateMachine = stateMachine;
        _context.aniBridge = GetComponent<PlayerAnimationBridge>();

        InitialState();
        _motor.Initialize(_context, config, _groundDetector);
        _context.aniBridge.Initialize(_context);
    }

    private void Start()
    {
        stateMachine.ChangeState(PlayerLocomotionStateId.Idle);
    }

    private void InitialState()
    {
        stateMachine.AddState(new PlayerIdleState(PlayerLocomotionStateId.Idle, _context, config));
        stateMachine.AddState(new PlayerMoveState(PlayerLocomotionStateId.Walk, _context, config));
        stateMachine.AddState(new PlayerMoveState(PlayerLocomotionStateId.Run, _context, config));
        stateMachine.AddState(new PlayerMoveState(PlayerLocomotionStateId.Sprint, _context, config));
        stateMachine.AddState(new PlayerDashState(PlayerLocomotionStateId.Dash, _context, config));
        stateMachine.AddState(new PlayerJumpState(PlayerLocomotionStateId.Jump, _context, config));
        stateMachine.AddState(new PlayerMoveStoppingState(PlayerLocomotionStateId.MoveStop, _context, config));
        stateMachine.AddState(new PlayerSprintImpulseState(PlayerLocomotionStateId.Sprint_Impulse, _context, config));
    }

    private void Update()
    {
        // 用于一些角色状态的改变
        SomeUpdate();

        _motor.UpdateGrounding();
        // 状态逻辑更新
        stateMachine.Update();
        _motor.ApplyMovement(Time.deltaTime);
        if (stateMachine.CurrentStateId == PlayerLocomotionStateId.Dash)
        {
            Debug.Log("player=>"+transform.position);
            Debug.Log("camera=>"+_context.camTransform.position);
        }
        // AnimatorStateInfo stateInfo = _context.animator.GetCurrentAnimatorStateInfo(0);
        // Debug.Log("当前 State Hash: " + stateInfo.shortNameHash);
    }

    private void SomeUpdate()
    {
        // 也许这个用事件会更好？不用每帧都判断
        _context.runModeEnabled = _context.inputReader.moveSwitchStartedThisFrame ? !_context.runModeEnabled:_context.runModeEnabled;
    }
}
