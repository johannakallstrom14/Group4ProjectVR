using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyingFruit : MonoBehaviour
{
    [Header("Fly Settings")]
    public Vector3 flyDirection = new Vector3(0, 1, 0); 
    public float speed = 2.0f;
    
    private bool shouldFly = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Denna anropas via "When Unselect" i din Interactable Unity Event Wrapper
    public void StartFlying()
    {
        if (shouldFly) return; // Förhindra att den triggas flera gånger

        shouldFly = true;
        rb.useGravity = false; // Stäng av tyngdkraften
        rb.isKinematic = false; // Se till att fysiken är aktiv så den kan röra sig

        // 1. Spela ljud via din AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayNextClip();
        }

        Debug.Log(gameObject.name + " is now flying!");
    }

    void FixedUpdate() // Bättre än Update för Rigidbody-rörelser
    {
        if (shouldFly)
        {
            // Vi sätter hastigheten direkt för en jämn flygning
            rb.linearVelocity = flyDirection.normalized * speed;
        }
    }
}