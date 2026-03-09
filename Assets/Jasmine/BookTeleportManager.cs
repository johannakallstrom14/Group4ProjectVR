using UnityEngine;
using System.Collections;

public class BookTeleportManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource bookAudio;
    public Transform playerTransform;
    public Transform targetLocation;
    public Material nextSkybox;

    private bool sequenceStarted = false;

    // This is the function you will link to your Fruit's "Select Exited" event
    public void OnFruitPickedUp()
    {
        if (!sequenceStarted)
        {
            sequenceStarted = true;
            Debug.Log("Fruit picked! Starting audio and teleport sequence...");
            StartCoroutine(WaitAndTeleport());
        }
    }

    IEnumerator WaitAndTeleport()
    {
        // Play the audio
        if (bookAudio != null)
        {
            bookAudio.Play();
            // Wait until the audio finishes
            yield return new WaitWhile(() => bookAudio.isPlaying);
        }

        // Perform teleportation
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