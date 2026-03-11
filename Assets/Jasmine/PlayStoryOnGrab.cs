using UnityEngine;

public class PlayStoryOnGrab : MonoBehaviour
{
    public AudioSource storyAudio;
    private bool hasPlayed = false;

    public void StartStory()
    {
        if (hasPlayed) return;

        hasPlayed = true;

        if (storyAudio != null)
        {
            storyAudio.Play();
            Debug.Log("Story started.");
        }
        else
        {
            Debug.LogWarning("No AudioSource assigned.");
        }
    }
}