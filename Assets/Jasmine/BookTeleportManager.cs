using UnityEngine;
using System.Collections;

public class BookTeleportManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource bookAudio;
    public Transform playerTransform; 
    public Transform targetLocation;  
    public Material nextSkybox;

    [Header("Transition Settings")]
    public float delayAfterAudio = 1.0f;
    private bool hasBeenTriggered = false;

    // This is the function we will link to the Meta Touch/Grab event
    public void OnBookTouched()
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            bookAudio.Play();
            StartCoroutine(WaitAndTeleport());
        }
    }

    IEnumerator WaitAndTeleport()
    {
        // Wait until the audio actually starts
        yield return new WaitUntil(() => bookAudio.isPlaying);

        // Wait until the audio stops playing
        yield return new WaitWhile(() => bookAudio.isPlaying);

        yield return new WaitForSeconds(delayAfterAudio);

        TeleportPlayer();
        ChangeEnvironment();
    }

    void TeleportPlayer()
    {
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false; 

        playerTransform.position = targetLocation.position;
        playerTransform.rotation = targetLocation.rotation;

        if (cc != null) cc.enabled = true; 
    }

    void ChangeEnvironment()
    {
        if (nextSkybox != null)
        {
            RenderSettings.skybox = nextSkybox;
            // Note: For Quest APK, keep an eye on performance with this line
            DynamicGI.UpdateEnvironment(); 
        }
    }
}