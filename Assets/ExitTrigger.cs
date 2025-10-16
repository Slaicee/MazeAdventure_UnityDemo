using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [Tooltip("玩家靠近时提示的按键")]
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
            Debug.Log("到达出口！游戏结束！");
            // 这里可以加入UI提示、场景切换、游戏结算等
        }
    }

    void OnGUI()
    {
        if (playerInside)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 30, 200, 40), "按 E 互动（离开迷宫）");
        }
    }
}
