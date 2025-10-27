using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("引用对象")]
    public GameObject player;

    private Vector3 startPosition;
    private Rigidbody rb;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            rb = player.GetComponent<Rigidbody>();
            startPosition = new Vector3(1.01f, player.transform.localScale.y / 2f, 1.01f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ResetPlayerPosition();
        }
    }

    void ResetPlayerPosition()
    {
        if (player == null) return;

        // 直接清零速度并修改位置，不触发物理异常
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = startPosition; 
        }
        else
        {
            player.transform.position = startPosition;
        }

        Debug.Log("玩家已回到起点（按 X 键触发）");
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 28; 
        GUI.color = Color.white;

        Vector2 shadowOffset = new Vector2(2, 2);
        string text1 = "按 X 键 回到起点";
        string text2 = "按 ESC 键 退出游戏";

        // 阴影层
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.Label(new Rect(12 + shadowOffset.x, 12 + shadowOffset.y, 400, 35), text1);
        GUI.Label(new Rect(12 + shadowOffset.x, 44 + shadowOffset.y, 400, 35), text2);

        // 主文字
        GUI.color = Color.white;
        GUI.Label(new Rect(10, 10, 400, 35), text1);
        GUI.Label(new Rect(10, 42, 400, 35), text2);
    }
}
