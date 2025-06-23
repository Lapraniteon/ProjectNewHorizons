using NaughtyAttributes;
using UnityEngine;

public class CollectableController : MonoBehaviour
{

    public Collectable[] collectables;
    
    public int Amount { get { return collectables.Length; } }

    [ProgressBar("Amount Collected", "Amount", EColor.Yellow)]
    public int AmountCollected;
    
    public bool AllCollected { get { return AmountCollected == Amount; } }
    
    void Start()
    {
        GameManager.Instance.currentCollectableController = this;

        foreach (Collectable collectable in collectables)
        {
            collectable.controller = this;
        }
    }

    public void GotCollectable(Collectable collectable)
    {
        AmountCollected++;
        
        if (AllCollected)
            Debug.Log("All collectables collected!");
    }

    [Button("Get Amount Collected")]
    private void PrintAmountCollected() => Debug.Log(AmountCollected);
}
