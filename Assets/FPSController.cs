using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 3f;
    [Range(0.05f, 1f)] public float moveSmooth = 0.15f;

    [Header("相机设置")]
    public Transform playerCamera;
    [Tooltip("鼠标水平灵敏度 (每像素角度)")]
    public float mouseSensitivityX = 1f;
    [Tooltip("鼠标垂直灵敏度 (每像素角度)")]
    public float mouseSensitivityY = 1f;
    public float minPitch = -60f;
    public float maxPitch = 40f;
    [Range(0.05f, 1f)] public float rotationSmooth = 0.6f;

    [Header("系统设置")]
    public bool lockCursor = true;
    public int targetFrameRate = 60;

    private Rigidbody rb;
    private float pitch = 0f;      // 当前俯仰角
    private float yaw = 0f;        // 当前水平角
    private Vector3 moveVelocity;  // 平滑速度

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 1;

        // 初始化角度
        yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        HandleMouseLook();
        HandleExit();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
        Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth);
        rb.MoveRotation(smoothRotation);

        playerCamera.localRotation = Quaternion.Slerp(
            playerCamera.localRotation,
            Quaternion.Euler(pitch, 0f, 0f),
            rotationSmooth
        );
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (transform.forward * vertical + transform.right * horizontal).normalized;
        Vector3 targetVelocity = moveDir * moveSpeed;
        targetVelocity.y = rb.velocity.y;

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 1f - Mathf.Exp(-moveSmooth * 20f * Time.fixedDeltaTime));
    }

    void HandleExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("退出游戏...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
