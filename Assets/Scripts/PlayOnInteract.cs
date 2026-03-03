using UnityEngine;

public class PlayOnInteract : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;

    public void PlayMusic()
    {
        if (!audioSource) return;

        Debug.Log("Book selected");

        // restart from beginning each time (optional)
        audioSource.Stop();
        audioSource.Play();
    }

    public void ToggleMusic()
    {
        if (!audioSource) return;

        if (audioSource.isPlaying) audioSource.Pause();
        else audioSource.UnPause();
    }
}
