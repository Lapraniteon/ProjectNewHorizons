using System.Collections.Generic;
using UnityEngine;

public class PsychoShroom : MonoBehaviour
{

    public List<GameObject> objectsToUnhide = new();

    void Start()
    {
        foreach (GameObject obj in objectsToUnhide)
            obj.SetActive(false);
    }
    
    public void Trigger()
    {
        foreach (GameObject obj in objectsToUnhide)
        {
            obj.SetActive(true);
        }
        
        gameObject.SetActive(false);
    }
}
