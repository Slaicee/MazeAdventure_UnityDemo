using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float moveSpeed = 2f;      // �ƶ��ٶ�
    public float lookSensitivity = 800f; // ���������

    [Header("�������")]
    public Transform playerCamera;    // �������ͷ�������
    public float minLookAngle = -60f;
    public float maxLookAngle = 40f;

    private float xRotation = 0f;     // ���������
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ����������ת����ֹ��ײ���½�ɫ��б
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }

        // �������
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

        // ���������ת
        transform.Rotate(Vector3.up * mouseX);

        // ���������ת
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

        // �������������ı�ˮƽ�ٶ�
        Vector3 velocity = rb.velocity;
        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;
        rb.velocity = velocity;
    }
}