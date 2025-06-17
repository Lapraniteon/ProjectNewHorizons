using UnityEngine;

public class Grapple : MonoBehaviour
{

    public TargetJoint2D targetJoint;
    public Vector2 targetJointTarget;
    public bool isGrappled = false;
    public bool isStationary = false;

    public Rigidbody2D body;
    public Vector2 forceTarget;
    public float forceMultiplier;
    public bool isGrappling;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /*if (isGrappling)
            {
                isGrappling = false;
                targetJoint.enabled = false;
                return;
            }*/
            
            Vector3 raycastTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
            
            Debug.Log(raycastTarget);

            Vector2 direction = raycastTarget - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f, LayerMask.GetMask("Grapple"));
            
            if (hit)
            {
                Debug.DrawRay(transform.position, direction, Color.red, 2f);
                DebugExtension.DebugPoint(hit.point, Color.cyan, 1f, 2f);

                targetJoint.enabled = true;
                targetJoint.target = hit.point;
                /*isGrappling = true;*/
                /*forceTarget = hit.point;
                isGrappling = true;*/
            }
        }

        /*if (Vector2.Distance(transform.position, forceTarget) < 1f && isGrappling)
        {
            isGrappling = false;
            body.linearVelocity /= 2f;
        }*/
    }

    /*void FixedUpdate()
    {
        if (isGrappling)
        {
            body.AddForce(forceTarget * forceMultiplier);
        }
    }*/
}
