using UnityEngine;
using System.Collections;

public class BookTeleportManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip teleportClip; // Drag your audio file here

    [Header("Teleport Settings")]
    public Transform playerTransform;
    public Transform targetLocation;
    public float delayAfterAudio = 0.5f; // Small buffer after sound ends

    [Header("Environment")]
    public Material nextSkybox;

    private bool sequenceStarted = false;

    public void StartTeleportSequence()
    {
        if (!sequenceStarted)
        {
            sequenceStarted = true;
            
            // Play through Global Manager for maximum clarity
            if (GlobalAudioManager.Instance != null)
            {
                GlobalAudioManager.Instance.PlayClip(teleportClip);
                StartCoroutine(WaitAndTeleport());
            }
            else
            {
                // Fallback if Manager is missing
                Debug.LogError("GlobalAudioManager instance not found!");
                TeleportPlayer(); 
            }
        }
    }

    IEnumerator WaitAndTeleport()
    {
        // Wait for the audio to finish playing in the global manager
        while (GlobalAudioManager.Instance.IsAudioPlaying())
        {
            yield return null;
        }

        // Optional extra delay so it's not a sudden "snap"
        yield return new WaitForSeconds(delayAfterAudio);

        TeleportPlayer();
        ChangeEnvironment();
    }

    void TeleportPlayer()
    {
        if (playerTransform == null || targetLocation == null) return;

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
            DynamicGI.UpdateEnvironment();
        }
    }
}