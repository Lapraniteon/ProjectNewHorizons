using UnityEngine;

public class CollectableController : MonoBehaviour
{

    public Collectable[] collectables;
    
    public int Amount { get { return collectables.Length; } }
    public int AmountCollected { get; private set; }
    
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
    }
}
