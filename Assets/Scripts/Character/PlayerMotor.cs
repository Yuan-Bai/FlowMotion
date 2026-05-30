
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GroundDetector))]
public sealed class PlayerMotor : MonoBehaviour
{
    [Header("Vertical Motion")]
    [SerializeField] private float groundedSnapVelocity = -2f;
    [SerializeField] private float maxFallSpeed = 50f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationDeadZone = 0.0001f;

    private PlayerContext _context;
    private PlayerLocomotionConfig _config;
    private GroundDetector _groundDetector;
    private CharacterController _controller;

    public void Initialize(PlayerContext context, PlayerLocomotionConfig config, GroundDetector groundDetector)
    {
        _context = context;
        _config = config;
        _groundDetector = groundDetector != null ? groundDetector : GetComponent<GroundDetector>();
        _controller = context?.controller != null ? context.controller : GetComponent<CharacterController>();

        if (_context != null && _controller != null)
        {
            _context.controller = _controller;
        }
    }

    public void UpdateGrounding()
    {
        if (_context == null)
        {
            return;
        }

        if (_groundDetector != null)
        {
            _groundDetector.DetectGround();
            _context.isGrounded = _groundDetector.IsGrounded;
        }
        else if (_controller != null)
        {
            _context.isGrounded = _controller.isGrounded;
        }
    }

    public void ApplyMovement(float deltaTime)
    {
        if (_context == null || _controller == null || deltaTime <= 0f)
        {
            return;
        }

        if (_context.gravityEnabled)
        {
            ApplyGravity(deltaTime);
        }
        MoveController(deltaTime);
        RotateToMoveDirection(deltaTime);
    }

    private void ApplyGravity(float deltaTime)
    {
        if (_context.isGrounded && _context.verticalVelocity <= 0f)
        {
            // 放在在斜坡抖动
            _context.verticalVelocity = groundedSnapVelocity;
            return;
        }

        float gravity = _config != null ? Mathf.Abs(_config.Gravity) : Mathf.Abs(Physics.gravity.y);
        _context.verticalVelocity -= gravity * deltaTime;
        _context.verticalVelocity = Mathf.Max(_context.verticalVelocity, -maxFallSpeed);
    }

    private void MoveController(float deltaTime)
    {
        if (_context.slopeAngle > _config.MaxAngle)
        {
            return;
        }
        if (_context.rootMotionPositionXZ && _context.rootMotionPositionY)
        {
            _controller.Move(_context.deltaPosition);
            _context.rootMotionVelocity = _context.deltaPosition / deltaTime;
        }
        else if (_context.rootMotionPositionXZ)
        {
            Vector3 deltaPosition = new(_context.deltaPosition.x, deltaTime*_context.verticalVelocity, _context.deltaPosition.z);
            _controller.Move(deltaPosition);
            _context.rootMotionVelocity = deltaPosition / deltaTime;
        }
        else if (_context.rootMotionPositionY)
        {
            Vector3 deltaPosition = new Vector3(0.0f, _context.deltaPosition.y, 0.0f) + _context.horizontalVelocity * deltaTime;
            _controller.Move(deltaPosition);
            _context.rootMotionVelocity = deltaPosition / deltaTime;
        }
        else
        {
            Vector3 velocity = _context.horizontalVelocity + Vector3.up * _context.verticalVelocity;
            _controller.Move(velocity * deltaTime);
        }
    }

    private void RotateToMoveDirection(float deltaTime)
    {
        if (_context.rootMotionRotation)
        {
            _context.root.rotation = _context.deltaRotation * _context.root.rotation;
        }
        else
        {
            if (_context.horizontalVelocity.sqrMagnitude < rotationDeadZone ||
                _context.moveDirection.sqrMagnitude < rotationDeadZone)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(_context.moveDirection, Vector3.up);
            _context.root.rotation = Quaternion.Slerp(
                _context.root.rotation,
                targetRotation,
                rotationSpeed * deltaTime);
        }
    }
}
