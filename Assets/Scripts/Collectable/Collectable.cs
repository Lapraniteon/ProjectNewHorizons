using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [HideInInspector] public CollectableController controller;

    public UnityEvent onCollect;
    
    public bool IsCollected { get; private set; }

    public void Collect()
    {
        controller.GotCollectable(this);
        gameObject.SetActive(false);
        IsCollected = true;
        
        onCollect.Invoke();
    }
    
    
}
