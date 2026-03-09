using UnityEngine;

public class PondMaterialSwap : MonoBehaviour
{
    [SerializeField] public Renderer pondRenderer;
    [SerializeField] public Material magicalMaterial;
    [SerializeField] public Transform fingertip;
    [SerializeField] public float touchDistance = 0.05f;


    private bool hasChanged = false;

    private void Reset()
    {
        pondRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(hasChanged || pondRenderer == null || fingertip == null)
        {
            return;
        }

        Vector3 closestPoint = pondRenderer.bounds.ClosestPoint(fingertip.position);
        float distance = Vector3.Distance(fingertip.position, closestPoint);

        if(distance <= touchDistance)
        {
            hasChanged = true;
            pondRenderer.material = magicalMaterial;
            Debug.Log("Pond changed material");
        }
    }
}
