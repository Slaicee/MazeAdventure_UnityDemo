using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private WinUIController winUI;
    private bool triggered = false;

    void Start()
    {
        // 自动在场景中找到 WinUIController
        /*
           当你在编辑器里把 Canvas 拖进 ExitTrigger（一个预制体）的 WinUI 引用栏时，
           实际上 Unity 是在 Prefab 资源文件 里保存了这个“场景对象的引用”。
           但是 Prefab 是 可复用资源，在运行时它会被克隆成一个新的实例，
           这个实例 不再知道场景里的 Canvas 是谁，所以引用自动变成 null。
         */
        winUI = FindObjectOfType<WinUIController>();
        if (winUI == null)
        {
            Debug.LogError("找不到 WinUIController，请检查场景中是否存在带此脚本的 Canvas！");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("到达出口！游戏结束！");

            if (winUI != null)
                winUI.ShowUI();

            // 可选：销毁光柱防止重复触发
            Destroy(gameObject);
        }
    }
}
