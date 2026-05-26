using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GroundDetector))]
[RequireComponent(typeof(PlayerMotor))]


public class Player : MonoBehaviour
{
    #region 人物状态参数
    [SerializeField] private PlayerLocomotionConfig config;
    #endregion

    #region 状态变量
    private StateMachine<PlayerLocomotionStateId> stateMachine;
    [SerializeField] private PlayerContext _context;
    #endregion

    #region Motor
    private GroundDetector _groundDetector;
    private PlayerMotor _motor;
    #endregion

    private Animator _animator;
    private PlayerInputReader _inputReader;

    private void Awake()
    {
        _groundDetector = GetComponent<GroundDetector>();
        _motor = GetComponent<PlayerMotor>();
        _animator = GetComponent<Animator>();
        _inputReader = GetComponent<PlayerInputReader>();

        stateMachine = new();
        _context = new();
        _context.inputReader = _inputReader;
        _context.root = transform;
        _context.controller = GetComponent<CharacterController>();
        _context.camTransform = Camera.main.transform;
        _context.stateMachine = stateMachine;
        _context.aniBridge = GetComponent<PlayerAnimationBridge>();
        _context.timeHub = new PlayerTimeHub(stateMachine);

        InitialState();
        _motor.Initialize(_context, config, _groundDetector);
        _context.aniBridge.Initialize(_animator, stateMachine, _context);
        // _context.aniBridge.Initialize(_context);
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
        stateMachine.AddState(new PlayerJumpSecondState(PlayerLocomotionStateId.JumpSecond, _context, config));
        stateMachine.AddState(new PlayerFallState(PlayerLocomotionStateId.Fall, _context, config));
        stateMachine.AddState(new PlayerLandState(PlayerLocomotionStateId.Land, _context, config));
        stateMachine.AddState(new PlayerTurnbackState(PlayerLocomotionStateId.Turnback, _context, config));

    }

    private void Update()
    {
        // 用于一些角色状态的改变
        UpdateRunModeToggle();
        _context.timeHub.Update(Time.deltaTime, Time.unscaledDeltaTime);
        _motor.UpdateGrounding();
        // 状态逻辑更新
        stateMachine.Update();
        _motor.ApplyMovement(Time.deltaTime);
        if (stateMachine.CurrentStateId == PlayerLocomotionStateId.Sprint || stateMachine.CurrentStateId == PlayerLocomotionStateId.Turnback)
        {
            Debug.Log("player=>"+transform.position);
        }
        // AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        // Debug.Log("当前 State Hash: " + stateInfo.shortNameHash);

        // 获取当前动画状态的详细信息
        // AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        // // 获取当前状态的名称 (如 "Base Layer.Run")
        // string currentStateName = stateInfo.fullPathHash.ToString(); // 通常使用哈希值判断
        // Debug.Log("当前动画状态完整路径: " + currentStateName);

        // // 如果想更直观地检查是否是特定状态，推荐使用 IsName 方法
        // // 例如，检查当前状态是否是 "Run"
        // if (stateInfo.IsName("Run")) 
        // {
        //     Debug.Log("当前在Run状态");
        // }

        // Debug.Log("player=>"+transform.position);
    }

    private void UpdateRunModeToggle()
    {
        if (_inputReader.moveSwitchStartedThisFrame)
        {
            _context.runModeEnabled = !_context.runModeEnabled;
        }
    }

}
