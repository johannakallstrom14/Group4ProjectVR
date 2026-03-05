using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DetachOnGrab : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void OnGrab()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
