using UnityEngine;
using System.Collections;

public class BookTeleportManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource bookAudio;
    public Transform playerTransform; // Drag your OVRCameraRig here
    public Transform targetLocation;  // Drag a Transform/Empty GameObj for destination
    public Material nextSkybox;

    [Header("Transition Settings")]
    public float delayAfterAudio = 1.0f; // Brief pause for dramatic effect

    void Start()
    {
        // Start the monitoring routine
        StartCoroutine(WaitAndTeleport());
    }

    IEnumerator WaitAndTeleport()
    {
        // 1. Wait until the audio actually starts playing (if it hasn't yet)
        yield return new WaitUntil(() => bookAudio.isPlaying);

        // 2. Wait until the audio stops playing
        yield return new WaitWhile(() => bookAudio.isPlaying);

        // 3. Small delay after the voice ends
        yield return new WaitForSeconds(delayAfterAudio);

        // 4. Perform the Teleport
        TeleportPlayer();

        // 5. Change the Skybox
        ChangeEnvironment();
    }
void TeleportPlayer()
{
    CharacterController cc = playerTransform.GetComponent<CharacterController>();
    if (cc != null) cc.enabled = false; // Disable temporarily to "warp"

    playerTransform.position = targetLocation.position;
    playerTransform.rotation = targetLocation.rotation;

    if (cc != null) cc.enabled = true; // Re-enable
}
  void ChangeEnvironment()
{
    if (nextSkybox != null)
    {
        RenderSettings.skybox = nextSkybox;
        // This is the crucial line:
        DynamicGI.UpdateEnvironment(); 
    }
    else 
    {
        Debug.LogError("Next Skybox is missing from the Inspector!");
    }
}
}