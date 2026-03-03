using System.Collections;
using UnityEngine;


public class TeleportAfterAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Teleport")]
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportationProvider;
    [SerializeField] private Transform destination; // TeleportDestination

    private Coroutine waitRoutine;

    // Call this when you start the music
    public void PlayMusicAndTeleportWhenDone()
    {
        if (!audioSource || !teleportationProvider || !destination) return;

        audioSource.Stop();
        audioSource.Play();

        if (waitRoutine != null) StopCoroutine(waitRoutine);
        waitRoutine = StartCoroutine(WaitThenTeleport());
    }

    private IEnumerator WaitThenTeleport()
    {
        // wait until the clip finishes
        while (audioSource.isPlaying)
            yield return null;

        var request = new UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportRequest
        {
            destinationPosition = destination.position,
            destinationRotation = destination.rotation
        };

        teleportationProvider.QueueTeleportRequest(request);
    }
}
