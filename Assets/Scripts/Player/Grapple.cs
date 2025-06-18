using UnityEngine;

public class Grapple : MonoBehaviour
{

    private Camera _mainCamera;

    public TargetJoint2D targetJoint;
    private Vector2 _touchStartPos;
    private Vector2 _touchStartPosWorldSpace;
    private Vector2 _direction;
    private Vector2 _directionWorldSpace;
    private bool _directionChosen;

    void Start()
    {
        _mainCamera = Camera.main;
    }
    
    void Update()
    {
        

        float cameraZOffset = transform.position.z - Camera.main.transform.position.z;
        
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    _touchStartPos = touch.position;
                    _touchStartPosWorldSpace = _mainCamera.ScreenToWorldPoint(new Vector3(_touchStartPos.x, _touchStartPos.y, cameraZOffset));
                    _directionChosen = false;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    _directionWorldSpace = (Vector2) _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraZOffset)) - _touchStartPosWorldSpace;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    _directionChosen = true;
                    break;
            }
        }
        if (_directionChosen)
        {
            Debug.Log("Launch!");
            RaycastAndMove(_directionWorldSpace);
            _directionChosen = false;
        }

        DebugExtension.DebugPoint(_touchStartPosWorldSpace, Color.cyan);
        Debug.DrawRay(_touchStartPosWorldSpace, _directionWorldSpace, _directionChosen ? Color.green : Color.red);
    }

    private void RaycastAndMove(Vector2 direction)
    {
        /*Vector3 raycastTarget = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));

        Debug.Log(raycastTarget);

        Vector2 direction = raycastTarget - transform.position;*/
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f, LayerMask.GetMask("Grapple"));

        if (hit)
        {
            Debug.DrawRay(transform.position, direction, Color.red, 2f);
            DebugExtension.DebugPoint(hit.point, Color.cyan, 1f, 2f);

            targetJoint.enabled = true;
            targetJoint.target = hit.point;
        }
    }
}
