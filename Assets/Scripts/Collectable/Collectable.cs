using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [HideInInspector] public CollectableController controller;

    public GameObject[] objectsToDisable;

    public UnityEvent onCollect;
    
    public bool IsCollected { get; private set; }

    public void Collect()
    {
        controller.GotCollectable(this);
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        IsCollected = true;
        
        onCollect.Invoke();
    }
    
    
}
