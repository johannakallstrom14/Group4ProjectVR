using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class MagicalVRMenu : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform playerCamera;   // Drag CenterEyeAnchor here
    public float distance = 2.5f;    // Distance from player
    public float followSpeed = 2.0f; // Speed of the glide
    public float maxAngle = 35.0f;   // Angle threshold before moving

    [Header("Timer Settings")]
    public float visibleDuration = 5.0f; // Seconds before starting fade
    public float fadeDuration = 2.0f;    // Seconds it takes to fade

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        // Reset alpha and start the fade timer whenever the menu is turned on
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeSequence());
    }

    void Update()
    {
        if (playerCamera == null) return;

        HandleFollow();
    }

    private void HandleFollow()
    {
        // Calculate where the menu "wants" to be in front of the player
        Vector3 targetPosition = playerCamera.position + (playerCamera.forward * distance);
        
        // Calculate the angle between the player's view and the current menu position
        float angle = Vector3.Angle(playerCamera.forward, transform.position - playerCamera.position);

        // If the player looks too far away, move the menu smoothly
        if (angle > maxAngle)
        {
            transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }

        // Always face the player
        transform.LookAt(playerCamera);
        transform.Rotate(0, 180, 0); // Flip so the text isn't mirrored
    }

    IEnumerator FadeSequence()
    {
        // 1. Wait while player reads the menu
        yield return new WaitForSeconds(visibleDuration);

        // 2. Smoothly fade the alpha to 0
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            yield return null;
        }

        // 3. Deactivate the object once it's invisible
        gameObject.SetActive(false);
    }
}