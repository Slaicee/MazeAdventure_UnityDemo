using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [Tooltip("��ҿ���ʱ��ʾ�İ���")]
    public KeyCode interactKey = KeyCode.E;

    private bool playerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
        {
            Debug.Log("������ڣ���Ϸ������");
            // ������Լ���UI��ʾ�������л�����Ϸ�����
        }
    }

    void OnGUI()
    {
        if (playerInside)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 30, 200, 40), "�� E �������뿪�Թ���");
        }
    }
}
