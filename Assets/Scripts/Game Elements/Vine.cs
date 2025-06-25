using UnityEngine;
using UnityEngine.Events;

public class Vine : MonoBehaviour
{
    public UnityEvent onGrappleAttach;
    public UnityEvent onGrappleDetach;
    
    private bool _hasGrappleAttached;

    public void GrappleAttach()
    {
        if (_hasGrappleAttached)
            return;

        _hasGrappleAttached = true;
        onGrappleAttach.Invoke();
    }

    public void GrappleDetach()
    {
        _hasGrappleAttached = false;
        onGrappleDetach.Invoke();
    } 
}
