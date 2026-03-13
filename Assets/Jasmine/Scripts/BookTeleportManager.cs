using UnityEngine;
using System.Collections;

public class BookTeleportManager : MonoBehaviour
{
    [Header("Animation")]
    public Animator bookAnimator;
    public string triggerName = "OpenBook";

    [Header("Audio Clips")]
    public AudioClip part1; // Plays at the start (at the book)
    public AudioClip part2; // Plays after teleporting (at the lake)

    [Header("Teleport Settings")]
    public Transform playerTransform;
    public Transform targetLocation;
    public float delayAfterPart1 = 0.5f; // Pause between Part 1 ending and teleporting

    [Header("Environment")]
    public Material nextSkybox;

    private bool sequenceStarted = false;

    public void StartTeleportSequence()
    {
        Debug.Log("Button was pushed!"); // This will show in the console
        if (!sequenceStarted)
        {
            sequenceStarted = true;

            // Force the animation by name if the trigger fails
            if (bookAnimator != null)
            {
                bookAnimator.Play("Armature|MagicBook_OpenAction"); // Use the EXACT name from your gray box
                Debug.Log("Playing Animation...");
            }

            if (GlobalAudioManager.Instance != null)
            {
                StartCoroutine(PlayFullStorySequence());
            }
        }
    }

    IEnumerator PlayFullStorySequence()
    {
        // --- PART 1: AT THE BOOK ---
        GlobalAudioManager.Instance.PlayClip(part1);

        // Wait until Part 1 is finished
        while (GlobalAudioManager.Instance.IsAudioPlaying())
        {
            yield return null;
        }

        // Buffer pause before the "jump"
        yield return new WaitForSeconds(delayAfterPart1);

        // --- THE TELEPORT ---
        ExecuteTeleport();
        ChangeEnvironment();

        // Brief moment for player to adjust eyes to new location
        yield return new WaitForSeconds(0.5f);

        // --- PART 2: AT THE NEW LOCATION ---
        GlobalAudioManager.Instance.PlayClip(part2);
    }

    void ExecuteTeleport()
    {
        if (playerTransform == null || targetLocation == null) return;

        CharacterController cc = playerTransform.GetComponent<CharacterController>();

        // Disable CC so it doesn't block the transform change
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