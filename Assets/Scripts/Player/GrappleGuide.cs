using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Grapple))]
public class GrappleGuide : MonoBehaviour
{

    private Grapple _grapple;

    public LineRenderer guideLineRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _grapple = GetComponent<Grapple>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_grapple._beganTouch && !_grapple._directionChosen)
        {
            RaycastHit2D hit = Physics2D.Raycast(_grapple.transform.position, _grapple._directionWorldSpace,
                _grapple.maxGrappleDistance, LayerMask.GetMask("Grapple", "Vine"));

            if (hit)
            {
                guideLineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                guideLineRenderer.SetPosition(1, _grapple.transform.position + (Vector3)_grapple._directionWorldSpace.normalized * _grapple.maxGrappleDistance);
            }
            
            guideLineRenderer.enabled = true;
        }
        else
        {
            guideLineRenderer.enabled = false;
        }
        
        guideLineRenderer.SetPosition(0, _grapple.transform.position);
    }
}
