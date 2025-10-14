using UnityEngine;

public class CameraAntiClip : MonoBehaviour
{
    public Transform player;       // 玩家主体
    public float minDistance = 0.1f; // 相机与墙的最小距离
    public LayerMask wallMask;     // 墙壁层

    private Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        // 从玩家中心向相机方向射线检测
        Vector3 worldHead = player.position + Vector3.up * 0.8f; // 头部高度
        Vector3 cameraWorldPos = transform.position;
        Vector3 dir = cameraWorldPos - worldHead;

        float dist = dir.magnitude;
        if (Physics.Raycast(worldHead, dir.normalized, out RaycastHit hit, dist, wallMask))
        {
            // 相机靠近墙 → 缩回一点
            Vector3 newPos = hit.point - dir.normalized * minDistance;
            transform.position = newPos;
        }
        else
        {
            // 没有碰撞 → 恢复默认相机位置
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * 10f);
        }
    }
}
