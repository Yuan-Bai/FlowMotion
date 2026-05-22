using UnityEngine;

[ExecuteInEditMode]
public class GroundDetector : MonoBehaviour
{
    [Header("检测设置")]
    [Tooltip("代表地面的层")]
    public LayerMask groundLayer = ~0;
    [Tooltip("射线检测距离")]
    public float checkDistance = 0.2f;
    [Tooltip("从角色脚底向上偏移的点")]
    public Vector3 rayOriginOffset = new Vector3(0, 0.5f, 0);

    public bool isGrounded = false;

    // 公共属性，方便其他组件（如角色控制器）读取
    public bool IsGrounded => isGrounded;

    public void DetectGround()
    {
        // 计算射线起点：角色世界坐标 + 偏移量
        Vector3 rayOrigin = transform.position + rayOriginOffset;
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, checkDistance, groundLayer);
    }

    // 在Scene窗口绘制辅助图形，实现可视化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 rayOrigin = transform.position + rayOriginOffset;
        Gizmos.DrawWireSphere(rayOrigin, checkDistance);
    }
}