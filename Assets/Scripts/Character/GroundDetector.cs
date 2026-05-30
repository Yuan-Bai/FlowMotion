using UnityEngine;

[ExecuteAlways]
public class GroundDetector : MonoBehaviour
{
    [Header("检测设置")]
    [Tooltip("代表地面的层")]
    public LayerMask groundLayer = ~0;
    [Tooltip("发射球体检测距离")]
    public float checkDistance = 0.1f;
    [Tooltip("发射球体的大小")]
    public float sphereRadius = 0.2f;
    [Tooltip("从角色脚底向上偏移的点")]
    public Vector3 rayOriginOffset = new Vector3(0, 0.5f, 0);
    [Tooltip("高度检测距离")]
    public float heightCheckDistance = 30f;

    public PlayerContext context;    
    public CharacterController controller;
    public Vector3 groundPoint;
    public Vector3 groundNormal;
    public Vector3 groundDistance;

    public bool isGrounded = false;

    // 公共属性，方便其他组件（如角色控制器）读取
    public bool IsGrounded => isGrounded;

    public void Initialize(PlayerContext context)
    {
        this.context = context;
        controller = context.controller;
    }

    public void DetectGround()
    {
        // 计算射线起点：角色世界坐标 + 偏移量
        Vector3 rayOrigin = transform.position + rayOriginOffset;
        isGrounded = Physics.SphereCast(rayOrigin, sphereRadius, Vector3.down, out RaycastHit hit, checkDistance, groundLayer);
        if (isGrounded)
        {
            context.groundNormal = hit.normal;
            context.slopeAngle = Vector3.Angle(hit.normal, transform.forward) - 90f;
        }
    }

    /// <summary>
    /// 应该比DetectGround()后执行
    /// </summary>
    public void UpdateHeight()
    {
        if (context == null) return;

        context.justLeftGround = false;
        context.justLanded = false;

        if (!isGrounded)
        {
            if (context.wasGroundedLastFrame)
            {
                context.justLeftGround = true;
                context.airborneStartY = controller.bounds.min.y;
                context.airborneMaxY = controller.bounds.min.y;
            }
            context.airborneMaxY = Mathf.Max(context.airborneMaxY, controller.bounds.min.y);
        }
        else
        {
            if (!context.wasGroundedLastFrame)
            {
                context.justLanded = true;
                context.landingY = controller.bounds.min.y;
                context.fallDistance = context.airborneMaxY - context.landingY;
            }
            context.airborneMaxY = 0.0f;
        }
        context.wasGroundedLastFrame = isGrounded;
    }

    // 在Scene窗口绘制辅助图形，实现可视化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 rayOrigin = transform.position + rayOriginOffset;
        Gizmos.DrawWireSphere(rayOrigin, sphereRadius);
        Gizmos.DrawWireSphere(rayOrigin + new Vector3(0f, -checkDistance, 0f), sphereRadius);
    }
}