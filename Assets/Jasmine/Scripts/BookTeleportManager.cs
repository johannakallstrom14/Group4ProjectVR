using UnityEngine;
using System.Collections;

public class BookTeleportManager : MonoBehaviour
{
    [Header("Animation")]
    public Animator bookAnimator;
    public string boolName = "OpenBook";
    public string triggerName = "BookOpen";

    [Header("Particles")]
    public ParticleSystem bookParticles;

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
        Debug.Log("Button was pushed!");

        if (!sequenceStarted)
        {
            sequenceStarted = true;

            if (bookAnimator != null)
            {
                bookAnimator.SetTrigger(triggerName);
                Debug.Log("Playing Animation...");
            }

            if (bookParticles != null)
            {
                bookParticles.Play();
            }

            if (GlobalAudioManager.Instance != null)
            {
                StartCoroutine(PlayFullStorySequence());
            }
        }
    }

    IEnumerator PlayFullStorySequence()
    {
        GlobalAudioManager.Instance.PlayClip(part1);

        while (GlobalAudioManager.Instance.IsAudioPlaying())
        {
            yield return null;
        }

        yield return new WaitForSeconds(delayAfterPart1);

        ExecuteTeleport();
        ChangeEnvironment();

        yield return new WaitForSeconds(0.5f);

        GlobalAudioManager.Instance.PlayClip(part2);
    }

    void ExecuteTeleport()
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
