using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinUIController : MonoBehaviour
{
    public CanvasGroup canvasGroup; // CanvasGroup 控制整体透明度
    public float fadeDuration = 1.5f; // 淡入时间

    void Start()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0; // 初始透明
    }

    public void ShowUI()
    {
        if (canvasGroup != null)
            StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
    }
}
