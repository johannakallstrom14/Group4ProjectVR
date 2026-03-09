using UnityEngine;
using Oculus.Interaction;

public class PondMaterialSwap : MonoBehaviour
{
    [SerializeField] public Renderer pondRenderer;
    [SerializeField] public Material normalMaterial;
    [SerializeField] public Material magicalMaterial;

    private bool hasChanged = false;

    private void Reset()
    {
        pondRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasChanged)
        {
            return;
        }

        //Check if the object touching the pond is a hand
        if (other.GetComponentInParent<PokeInteractor>() != null)
        {
            hasChanged = true;
            pondRenderer.material = magicalMaterial;
            Debug.Log("Pond changed material");
        }
    }
}
