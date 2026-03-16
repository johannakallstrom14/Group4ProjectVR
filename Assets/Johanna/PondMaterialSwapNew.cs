using UnityEngine;

public class PondTouchByDistance : MonoBehaviour
{
    [SerializeField] private Renderer pondRenderer;
    [SerializeField] private Material magicalMaterial;
    [SerializeField] private Transform leftFingertip;
    [SerializeField] private Transform rightFingertip;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private float touchDistance = 0.1f;
     
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // The "Player"
    public AudioClip teleportClip; // The "File"

    private bool hasChanged = false;

    private void Update()
    {
        if (hasChanged || pondRenderer == null || magicalMaterial == null || triggerCollider == null)
            return;

        CheckFinger(leftFingertip);
        CheckFinger(rightFingertip);
    }

    private void CheckFinger(Transform fingertip)
    {
        if (fingertip == null) return;

        Vector3 closestPoint = triggerCollider.ClosestPoint(fingertip.position);
        float distance = Vector3.Distance(fingertip.position, closestPoint);

        if (distance <= touchDistance)
        {
            ChangePondState();
        }
    }

    private void ChangePondState()
    {
        hasChanged = true;
        
        // 1. Change the visual
        pondRenderer.material = magicalMaterial;
        
        // 2. Play the audio at the exact same time
        if (audioSource != null && teleportClip != null)
        {
            audioSource.PlayOneShot(teleportClip);
        }

        Debug.Log("Pond material changed and audio played.");
    }
}