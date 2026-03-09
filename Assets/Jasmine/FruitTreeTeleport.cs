using UnityEngine;
using System.Collections;

public class FruitTreeTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform playerTransform;    // Drag XR Origin or Main Camera here
    public Transform lakeLocation;       // Drag an empty GameObject at the lake here
    public float delayBeforeTeleport = 2.0f;

    [Header("Environment")]
    public AudioSource teleportAudio;    // The audio that plays when you grab the fruit
    public Material lakeSkybox;          // The skybox material for the lake

    private bool hasTeleported = false;

    // This is the function you link to the Cherry's "Select Exited" event
    public void OnCherryGrabbed()
    {
        if (!hasTeleported)
        {
            hasTeleported = true;
            Debug.Log("Cherry grabbed! Starting delay sequence...");
            StartCoroutine(TeleportSequence());
        }
    }

    IEnumerator TeleportSequence()
    {
        // 1. Play the audio immediately upon grabbing
        if (teleportAudio != null)
        {
            teleportAudio.Play();
        }

        // 2. Wait for the 2-second delay
        yield return new WaitForSeconds(delayBeforeTeleport);

        // 3. Perform the teleport
        ExecuteTeleport();

        // 4. Change the skybox
        if (lakeSkybox != null)
        {
            RenderSettings.skybox = lakeSkybox;
            DynamicGI.UpdateEnvironment();
        }
    }

    private void ExecuteTeleport()
    {
        if (playerTransform == null || lakeLocation == null) return;

        // Disable CharacterController to allow the position change
        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerTransform.position = lakeLocation.position;
        playerTransform.rotation = lakeLocation.rotation;

        // Re-enable CharacterController
        if (cc != null) cc.enabled = true;
    }
}