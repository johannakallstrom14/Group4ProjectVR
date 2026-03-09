using UnityEngine;
using System.Collections;

public class AudioStation : MonoBehaviour
{
    public AudioSource stationAudio;
    public GameObject diamondObject; // The diamond for THIS station
    public DiamondManager manager;

    private bool audioFinished = false;
    private bool diamondCollected = false;

    void Start()
    {
        // Hide the diamond at the start
        if (diamondObject != null) diamondObject.SetActive(false);

        // Start checking for audio completion
        StartCoroutine(WaitForAudio());
    }

    IEnumerator WaitForAudio()
    {
        // Wait until the audio actually starts playing (if there's a delay)
        yield return new WaitUntil(() => stationAudio.isPlaying);

        // Wait until the audio stops
        yield return new WaitUntil(() => !stationAudio.isPlaying);

        audioFinished = true;
        SpawnDiamond();
    }

    void SpawnDiamond()
    {
        if (diamondObject != null)
        {
            diamondObject.SetActive(true);
            Debug.Log("Audio finished. Diamond is now collectible!");
        }
    }

    // Call this function from your VR "Grab" event or a Trigger
    public void CollectDiamond()
    {
        if (audioFinished && !diamondCollected)
        {
            diamondCollected = true;
            diamondObject.SetActive(false); // Make it disappear
            manager.AddDiamond();           // Update the master count
        }
    }

    // Optional: If you use a simple Trigger to "collect" by walking into it
    private void OnTriggerEnter(Collider foreignBody)
    {
        if (foreignBody.CompareTag("Player"))
        {
            CollectDiamond();
        }
    }
}