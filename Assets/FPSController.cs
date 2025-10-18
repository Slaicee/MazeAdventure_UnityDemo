using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 3f;
    [Tooltip("ÿ����ת�ĽǶȣ�����ÿ֡����ͳһ�����Ȼ�׼")]
    public float lookAnglePerSecond = 120f; // �ؼ������á�ÿ��Ƕȡ�����������
    [Range(0.1f, 1f)] public float rotateSmooth = 0.8f;

    [Header("�������")]
    public Transform playerCamera;
    public float minLookAngle = -60f;
    public float maxLookAngle = 40f;

    private Rigidbody rb;
    private float xRotation = 0f;
    private float mouseXAccumulator;        // �ۻ����X����
    private float targetYaw;                // Ŀ��ˮƽ��ת�Ƕ�

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
        // 1. ����������ԭʼֵ�����ٲ�ͬ�����µ�ƽ�����죩
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // 2. ��ֱ��ת�����ڡ�ÿ��Ƕȡ����㣬����֡��Ӱ��
        float verticalRotateDelta = mouseY * lookAnglePerSecond * Time.deltaTime;
        xRotation = Mathf.Lerp(xRotation, xRotation - verticalRotateDelta, rotateSmooth * 10f);
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. ˮƽ��ת�����ۻ���ͬ������ÿ��Ƕȣ�
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
            // ƽ����ת��ȷ����������Ⱦͬ��
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
        Debug.Log("�˳���Ϸ...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}