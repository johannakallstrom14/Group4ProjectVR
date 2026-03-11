using UnityEngine;
using System.Collections;

public class BerrySequenceManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource narrationAudio;
    public Transform playerTransform;
    public Transform lakeLocation;

    private bool audioStarted = false;
    private bool berryReleased = false;
    private bool narrationFinished = false;
    private bool hasTeleported = false;

    public void OnBerryGrabbed()
    {
        if (!audioStarted && narrationAudio != null)
        {
            audioStarted = true;
            narrationAudio.Play();
            StartCoroutine(WaitForNarrationToFinish());
            Debug.Log("Narration started.");
        }
    }

    public void OnBerryReleased()
    {
        berryReleased = true;
        Debug.Log("Berry released.");

        TryTeleport();
    }

    private IEnumerator WaitForNarrationToFinish()
    {
        while (narrationAudio != null && narrationAudio.isPlaying)
        {
            yield return null;
        }

        narrationFinished = true;
        Debug.Log("Narration finished.");

        TryTeleport();
    }

    private void TryTeleport()
    {
        if (hasTeleported) return;

        if (berryReleased && narrationFinished)
        {
            ExecuteTeleport();
        }
    }

    private void ExecuteTeleport()
    {
        if (playerTransform == null || lakeLocation == null)
        {
            Debug.LogWarning("Missing playerTransform or lakeLocation.");
            return;
        }

        hasTeleported = true;

        CharacterController cc = playerTransform.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        playerTransform.position = lakeLocation.position;
        playerTransform.rotation = lakeLocation.rotation;

        if (cc != null) cc.enabled = true;

        Debug.Log("Player teleported.");
    }
}
