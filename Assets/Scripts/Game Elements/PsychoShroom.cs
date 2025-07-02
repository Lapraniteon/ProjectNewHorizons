using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PsychoShroom : MonoBehaviour
{

    public List<GameObject> objectsToUnhide = new();

    public UnityEvent onTrigger;

    private bool isCollected = false;

    void Start()
    {
        foreach (GameObject obj in objectsToUnhide)
            obj.SetActive(false);
    }
    
    public void Trigger()
    {
        if (isCollected)
            return;
        
        foreach (GameObject obj in objectsToUnhide)
        {
            obj.SetActive(true);
        }
        
        //gameObject.SetActive(false);

        onTrigger.Invoke();

        isCollected = true;
    }
}
