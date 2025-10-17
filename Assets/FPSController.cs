using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 2f;      // 移动速度
    public float lookSensitivity = 800f; // 鼠标灵敏度

    [Header("相机设置")]
    public Transform playerCamera;    // 挂在玩家头部的相机
    public float minLookAngle = -60f;
    public float maxLookAngle = 40f;

    private float xRotation = 0f;     // 相机俯仰角
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 冻结所有旋转，防止碰撞导致角色歪斜
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }

        // 锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        // 玩家左右旋转
        transform.Rotate(Vector3.up * mouseX);

        // 相机上下旋转
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        moveDir.Normalize();

        // 保留重力，仅改变水平速度
        Vector3 velocity = rb.velocity;
        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;
        rb.velocity = velocity;
    }
}