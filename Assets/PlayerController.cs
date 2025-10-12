using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //移动参数
    [Header("移动设置")]
    public float MoveSpeed = 5f;//速度
    public float RotationSpeedX = 100f;//旋转灵敏度
    public float RotationSpeedY = 80f;

    //相机引用
    [Header("相机设置")]
    public Camera PlayerCamera;
    public float MaxLookAngle = 85f;//仰视
    public float MinLookAngle = -85f;//俯视
    private float XRotation;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerCamera == null)
        {
            PlayerCamera = Camera.main;
        }

        //锁定鼠标并隐藏
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //处理鼠标旋转
        HandleMouseRotation();
        //处理键盘移动
        HandleKeyboardMovement();
    }

    void HandleMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * RotationSpeedX * Time.deltaTime;
        // 角色左右旋转（Y轴）：鼠标X轴控制角色转向
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleKeyboardMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 基于角色当前朝向计算移动方向：
        // forward = 角色前方（Z轴），right = 角色右方（X轴）
        Vector3 MoveDir = transform.forward * vertical + transform.right * horizontal;
        MoveDir.y = 0f; // 忽略Y轴（避免上下移动，适合地面角色）
        MoveDir.Normalize(); // 归一化向量，避免斜向移动速度过快

        transform.Translate(MoveDir * MoveSpeed * Time.deltaTime, Space.World);
    }
}
