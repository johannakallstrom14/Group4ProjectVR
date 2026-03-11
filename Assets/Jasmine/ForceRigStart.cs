using UnityEngine;

public class ForceRigStart : MonoBehaviour
{
    public Transform startPoint;

    void Start()
    {
        // Move whole rig to the start point
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;

        // Wait one frame to override XR tracking offset
        StartCoroutine(FixNextFrame());
    }

    System.Collections.IEnumerator FixNextFrame()
    {
        yield return null;

        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }
}