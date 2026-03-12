using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // Prevent duplicates

        audioSource = GetComponent<AudioSource>();
        
        // Settings for crisp, loud VR audio
        audioSource.spatialBlend = 0; // 2D (both ears)
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
    }

    // This is the name the Teleport script is looking for
    public void PlayClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public bool IsAudioPlaying()
    {
        return audioSource.isPlaying;
    }
}