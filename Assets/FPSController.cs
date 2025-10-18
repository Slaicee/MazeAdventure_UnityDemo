using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 3f;
    [Tooltip("每秒旋转的角度（而非每帧），统一灵敏度基准")]
    public float lookAnglePerSecond = 120f; // 关键：改用“每秒角度”定义灵敏度
    [Range(0.1f, 1f)] public float rotateSmooth = 0.8f;

    [Header("相机设置")]
    public Transform playerCamera;
    public float minLookAngle = -60f;
    public float maxLookAngle = 40f;

    private Rigidbody rb;
    private float xRotation = 0f;
    private float mouseXAccumulator;        // 累积鼠标X增量
    private float targetYaw;                // 目标水平旋转角度

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetYaw = transform.eulerAngles.y;
    }

    void Update()
    {
        // 1. 鼠标输入采用原始值（减少不同环境下的平滑差异）
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // 2. 垂直旋转：基于“每秒角度”计算，不受帧率影响
        float verticalRotateDelta = mouseY * lookAnglePerSecond * Time.deltaTime;
        xRotation = Mathf.Lerp(xRotation, xRotation - verticalRotateDelta, rotateSmooth * 10f);
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. 水平旋转增量累积（同样基于每秒角度）
        mouseXAccumulator += mouseX * lookAnglePerSecond * Time.deltaTime;

        CheckQuit();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleHorizontalRotation();
    }

    void HandleHorizontalRotation()
    {
        if (Mathf.Abs(mouseXAccumulator) > 0.001f)
        {
            targetYaw += mouseXAccumulator;
            // 平滑旋转，确保物理与渲染同步
            float currentYaw = transform.eulerAngles.y;
            float smoothedYaw = Mathf.LerpAngle(currentYaw, targetYaw, rotateSmooth);
            rb.MoveRotation(Quaternion.Euler(0f, smoothedYaw, 0f));

            mouseXAccumulator = 0;
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.forward * vertical + transform.right * horizontal;
        if (moveDir.magnitude > 1f) moveDir.Normalize();

        Vector3 targetVelocity = moveDir * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, rotateSmooth * 5f);
    }

    void CheckQuit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
        Debug.Log("退出游戏...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}