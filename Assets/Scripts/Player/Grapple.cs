using UnityEngine;

public class Grapple : MonoBehaviour
{

    public TargetJoint2D targetJoint;
    public Vector2 targetJointTarget;
    public bool isGrappled = false;
    public bool isStationary = false;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 raycastTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            
            Debug.Log(raycastTarget);

            Vector2 direction = raycastTarget - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, LayerMask.GetMask("Grapple"));
            
            if (hit)
            {
                Debug.DrawRay(transform.position, direction, Color.red, 2f);
                DebugExtension.DebugPoint(hit.point, Color.cyan, 1f, 2f);

                targetJoint.enabled = true;
                targetJoint.target = hit.point;
            }
        }
    }
}
