using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableController controller;
    
    public bool IsCollected { get; private set; }

    public void Collect()
    {
        controller.GotCollectable(this);
        gameObject.SetActive(false);
        IsCollected = true;
    }
}
