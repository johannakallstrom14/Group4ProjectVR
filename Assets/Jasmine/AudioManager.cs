using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton to make it easy to call from other scripts
    public AudioSource audioSource;
    public AudioClip[] clips;
    private int currentClipIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayNextClip()
    {
        if (clips.Length == 0 || audioSource == null) return;

        if (currentClipIndex < clips.Length)
        {
            audioSource.clip = clips[currentClipIndex];
            audioSource.Play();
            currentClipIndex++;
        }
        else
        {
            Debug.Log("No more audio clips in the sequence.");
        }
    }
}