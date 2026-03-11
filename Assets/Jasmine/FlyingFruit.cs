using UnityEngine;

public class FlyingFruit : MonoBehaviour
{
    public Vector3 flyDirection = new Vector3(0, 1, 0); // Flyger uppåt som standard
    public float speed = 2.0f;
    private bool shouldFly = false;

    // Denna anropas via din Event Wrapper när du släpper frukten
    public void StartFlying()
    {
        shouldFly = true;
        // Valfritt: Stäng av tyngdkraften så den inte faller ner
        GetComponent<Rigidbody>().useGravity = false;
    }

    void Update()
    {
        if (shouldFly)
        {
            transform.Translate(flyDirection * speed * Time.deltaTime);
        }
    }
}