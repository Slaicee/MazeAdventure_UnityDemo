using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMMulti : MonoBehaviour
{
    public AudioClip[] clips;       // 放所有 BGM
    public float volume = 0.3f;

    private AudioSource audioSource;
    private int currentIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.loop = false;   // 我们用脚本控制循环
        PlayNext();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNext();
        }
    }

    void PlayNext()
    {
        if (clips.Length == 0) return;

        audioSource.clip = clips[currentIndex];
        audioSource.Play();

        currentIndex++;
        if (currentIndex >= clips.Length)
            currentIndex = 0; // 循环播放
    }
}
