using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMMulti : MonoBehaviour
{
    public AudioClip[] clips;       // ������ BGM
    public float volume = 0.3f;

    private AudioSource audioSource;
    private int currentIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.loop = false;   // �����ýű�����ѭ��
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
            currentIndex = 0; // ѭ������
    }
}
