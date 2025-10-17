using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinUIController : MonoBehaviour
{
    public CanvasGroup canvasGroup; // CanvasGroup ��������͸����
    public float fadeDuration = 1.5f; // ����ʱ��

    void Start()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0; // ��ʼ͸��
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
