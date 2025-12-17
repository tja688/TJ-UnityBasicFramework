using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("檢測設定")]
    public Transform groundCheckPos;   // 放置在腳底的空物體
    public float groundDistance = 0.2f; // 檢測圓球的半徑
    public LayerMask groundMask;       // 指定什麼圖層是「地面」

    public bool isGrounded;

    void Update()
    {
        // 核心代碼：檢測腳底位置，指定半徑內是否有 groundMask 圖層的物體
        isGrounded = Physics.CheckSphere(groundCheckPos.position, groundDistance, groundMask);
    }

    // 輔助視覺化：在 Scene 視窗畫出檢測球，方便除錯
    void OnDrawGizmosSelected()
    {
        if (groundCheckPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPos.position, groundDistance);
        }
    }
}