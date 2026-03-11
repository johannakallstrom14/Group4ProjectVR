using UnityEngine;

public class PondTouchByDistance : MonoBehaviour
{
    [SerializeField] private Renderer pondRenderer;
    [SerializeField] private Material magicalMaterial;
    [SerializeField] private Transform leftFingertip;
    [SerializeField] private Transform rightFingertip;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private float touchDistance = 0.1f;

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
            hasChanged = true;
            pondRenderer.material = magicalMaterial;
            Debug.Log("Pond changed material");
        }
    }
}
