using UnityEngine;
using UnityEngine.Events;

public partial class DiamondManager : MonoBehaviour
{
    public int diamondsCollected = 0;
    public int totalRequired = 3;

    // This event fires when all diamonds are found
    public UnityEvent onAllDiamondsCollected;

    public void AddDiamond()
    {
        diamondsCollected++;
        Debug.Log($"Diamonds Collected: {diamondsCollected}/{totalRequired}");

        if (diamondsCollected >= totalRequired)
        {
            Debug.Log("All diamonds collected! Final step unlocked.");
            onAllDiamondsCollected.Invoke();
        }
    }
}