using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("玩家引用")]
    public Transform player;

    [Header("相机参数")]
    public Vector3 offset = new Vector3(0, 2, -5);      // 相机相对玩家的偏移
    public Vector3 LookAtOffset = new Vector3(0, 1.5f, 0); // 相机看向玩家头部偏移

    [Header("旋转设置")]
    public float sensitivityY = 80f;  // 鼠标Y灵敏度（俯仰）
    public float rotationSmoothTime = 0.1f;
    public float minPitch = -45f;     // 最低俯角
    public float maxPitch = 45f;      // 最高仰角

    private float pitch = 0f;         // 当前俯仰角
    private float currentYRotation;   // 当前Y轴角度（用于平滑左右转）
    private Vector3 rotationVelocity;

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("请为相机指定玩家对象！");
            return;
        }

        // ---- 1️⃣ 读取鼠标输入（只控制俯仰）----
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        pitch -= mouseY; // 鼠标上移 → 仰视
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // ---- 2️⃣ 同步玩家左右旋转（由 PlayerController 控制）----
        currentYRotation = player.eulerAngles.y;

        // ---- 3️⃣ 计算相机旋转与位置 ----
        Quaternion rotation = Quaternion.Euler(pitch, currentYRotation, 0f);
        Vector3 targetPosition = player.position + rotation * offset;

        transform.position = targetPosition;
        transform.LookAt(player.position + LookAtOffset);
    }
}
