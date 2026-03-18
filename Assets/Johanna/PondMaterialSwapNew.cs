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
    [SerializeField] private AudioSource audioSource;
    public AudioClip teleportClip;

    [Header("Canvas Settings")]
    [SerializeField] private GameObject canvasObject; // 👈 ADD THIS

    private bool hasChanged = false;

    private void Start()
    {
        // Hide canvas at start
        if (canvasObject != null)
            canvasObject.SetActive(false);
    }

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

        // 1. Change visual
        pondRenderer.material = magicalMaterial;

        // 2. Play audio
        if (audioSource != null && teleportClip != null)
        {
            audioSource.PlayOneShot(teleportClip);
        }

        // 3. Show canvas 👈 ADD THIS
        if (canvasObject != null)
        {
            canvasObject.SetActive(true);
        }

        Debug.Log("Pond material changed, audio played, canvas shown.");
    }
}