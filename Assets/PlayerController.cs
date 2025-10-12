using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�ƶ�����
    [Header("�ƶ�����")]
    public float MoveSpeed = 5f;//�ٶ�
    public float RotationSpeedX = 100f;//��ת������
    public float RotationSpeedY = 80f;

    //�������
    [Header("�������")]
    public Camera PlayerCamera;
    public float MaxLookAngle = 85f;//����
    public float MinLookAngle = -85f;//����
    private float XRotation;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerCamera == null)
        {
            PlayerCamera = Camera.main;
        }

        //������겢����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //���������ת
        HandleMouseRotation();
        //��������ƶ�
        HandleKeyboardMovement();
    }

    void HandleMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * RotationSpeedX * Time.deltaTime;
        // ��ɫ������ת��Y�ᣩ�����X����ƽ�ɫת��
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleKeyboardMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ���ڽ�ɫ��ǰ��������ƶ�����
        // forward = ��ɫǰ����Z�ᣩ��right = ��ɫ�ҷ���X�ᣩ
        Vector3 MoveDir = transform.forward * vertical + transform.right * horizontal;
        MoveDir.y = 0f; // ����Y�ᣨ���������ƶ����ʺϵ����ɫ��
        MoveDir.Normalize(); // ��һ������������б���ƶ��ٶȹ���

        transform.Translate(MoveDir * MoveSpeed * Time.deltaTime, Space.World);
    }
}
