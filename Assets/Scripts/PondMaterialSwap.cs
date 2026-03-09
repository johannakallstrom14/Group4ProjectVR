using UnityEngine;

public class PondMaterialSwap : MonoBehaviour
{
    [SerializeField] public Renderer pondRenderer;
    [SerializeField] public Material magicalMaterial;
    [SerializeField] public Transform fingertip;
    [SerializeField] public BoxCollider triggerCollider;

    [SerializeField] public float touchDistance = 0.15f;


    private bool hasChanged = false;

    /*private void Reset()
    {
        pondRenderer = GetComponent<Renderer>();
    }*/

    private void Update()
    {
        if(hasChanged || pondRenderer == null || magicalMaterial == null || fingertip == null || triggerCollider == null)
        {
            return;
        }

        Vector3 closestPoint = triggerCollider.ClosestPoint(fingertip.position);
        float distance = Vector3.Distance(fingertip.position, closestPoint);

        if(distance <= touchDistance)
        {
            hasChanged = true;
            pondRenderer.material = magicalMaterial;
            Debug.Log("Pond changed material");
        }
    }
}
