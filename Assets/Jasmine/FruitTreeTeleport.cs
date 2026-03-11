using UnityEngine;
using System.Collections;

public class FruitTreeTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform playerTransform;    // Drag XR Origin here
    public Transform lakeLocation;       // Drag empty GameObject at the lake
    public float delayBeforeTeleport = 2.0f;

    [Header("Environment")]
    public Material lakeSkybox;          // The skybox material for the lake

    private bool hasTeleported = false;

    // Trigger this from the Meta SDK Grabbable 'When Select Entered' event
    public void OnCherryGrabbed()
    {
        if (!hasTeleported)
        {
            hasTeleported = true;
            Debug.Log("Cherry grabbed! Triggering Audio and Delay...");

            // 1. Tell the AudioManager to play the next sound
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayNextClip();
            }

            // 2. Start the timer for the teleport
            StartCoroutine(TeleportSequence());
        }
    }

    IEnumerator TeleportSequence()
    {
        // Wait for the 2-second delay
        yield return new WaitForSeconds(delayBeforeTeleport);

        // Perform the teleport
        ExecuteTeleport();

        // Change the skybox
        if (lakeSkybox != null)
        {
            RenderSettings.skybox = lakeSkybox;
            DynamicGI.UpdateEnvironment();
        }
    }

    private void ExecuteTeleport()
    {
        if (playerTransform == null || lakeLocation == null) return;

        // Disable CharacterController if present to prevent physics fighting the teleport
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerTransform.position = lakeLocation.position;
        playerTransform.rotation = lakeLocation.rotation;

        if (cc != null) cc.enabled = true;
        Debug.Log("Teleport Complete.");
    }
}