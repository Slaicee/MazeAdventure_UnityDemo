using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private WinUIController winUI;
    private bool triggered = false;

    void Start()
    {
        // �Զ��ڳ������ҵ� WinUIController
        /*
           �����ڱ༭����� Canvas �Ͻ� ExitTrigger��һ��Ԥ���壩�� WinUI ������ʱ��
           ʵ���� Unity ���� Prefab ��Դ�ļ� �ﱣ���������������������á���
           ���� Prefab �� �ɸ�����Դ��������ʱ���ᱻ��¡��һ���µ�ʵ����
           ���ʵ�� ����֪��������� Canvas ��˭�����������Զ���� null��
         */
        winUI = FindObjectOfType<WinUIController>();
        if (winUI == null)
        {
            Debug.LogError("�Ҳ��� WinUIController�����鳡�����Ƿ���ڴ��˽ű��� Canvas��");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("������ڣ���Ϸ������");

            if (winUI != null)
                winUI.ShowUI();

            // ��ѡ�����ٹ�����ֹ�ظ�����
            Destroy(gameObject);
        }
    }
}
