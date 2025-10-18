using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinUIController : MonoBehaviour
{
    public CanvasGroup canvasGroup;       // 控制UI透明度
    public float fadeDuration = 1.5f;     // 淡入时间
    public float waitBeforeExit = 2f;     // 显示后等待几秒再退出

    void Start()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0; // 初始透明
    }

    public void ShowUI()
    {
        if (canvasGroup != null)
            StartCoroutine(ShowAndExit());
    }

    private IEnumerator ShowAndExit()
    {
        // 1️⃣ 淡入动画
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        // 2️⃣ 等待几秒显示胜利界面
        yield return new WaitForSeconds(waitBeforeExit);

        // 3️⃣ 退出游戏（区分编辑器和打包后）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止运行
#else
        Application.Quit(); // 在打包后的游戏中退出
#endif
    }
}
