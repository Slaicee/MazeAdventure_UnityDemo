using UnityEngine;

public class CameraAntiClip : MonoBehaviour
{
    public Transform player;       // �������
    public float minDistance = 0.1f; // �����ǽ����С����
    public LayerMask wallMask;     // ǽ�ڲ�

    private Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        // ���������������������߼��
        Vector3 worldHead = player.position + Vector3.up * 0.8f; // ͷ���߶�
        Vector3 cameraWorldPos = transform.position;
        Vector3 dir = cameraWorldPos - worldHead;

        float dist = dir.magnitude;
        if (Physics.Raycast(worldHead, dir.normalized, out RaycastHit hit, dist, wallMask))
        {
            // �������ǽ �� ����һ��
            Vector3 newPos = hit.point - dir.normalized * minDistance;
            transform.position = newPos;
        }
        else
        {
            // û����ײ �� �ָ�Ĭ�����λ��
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * 10f);
        }
    }
}
