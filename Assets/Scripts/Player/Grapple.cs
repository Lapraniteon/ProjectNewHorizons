using UnityEngine;
using DG.Tweening;

public class Grapple : MonoBehaviour
{

    private Camera _mainCamera;

    public TargetJoint2D targetJoint;
    public float maxGrappleDistance;
    private Vector2 _touchStartPos;
    private Vector2 _touchStartPosWorldSpace;
    private Vector2 _direction;
    private Vector2 _directionWorldSpace;
    private bool _directionChosen;

    public Rigidbody2D body;
    public LineRenderer lineRenderer;

    private Tweener _timeTweener;
    private float _startingFixedDeltaTime;

    void Start()
    {
        _startingFixedDeltaTime = Time.fixedDeltaTime;
        _mainCamera = Camera.main;
    }
    
    void Update()
    {
        
        float cameraZOffset = transform.position.z - Camera.main.transform.position.z;
        
        Time.fixedDeltaTime = _startingFixedDeltaTime * Time.timeScale;
        
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
                    _timeTweener = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.1f, 0.5f).SetEase(Ease.OutCubic);
                    break;
                
                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    Vector2 touchPositionWorldSpace =
                        _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraZOffset));
                    
                    _directionWorldSpace = touchPositionWorldSpace - _touchStartPosWorldSpace;

                    if (Vector2.Distance(_touchStartPosWorldSpace, touchPositionWorldSpace) > 4f)
                        _directionChosen = true;
                    
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
            
            _timeTweener.Kill();
            Time.timeScale = 1f;
            
            RaycastAndMove(_directionWorldSpace);
            _directionChosen = false;
        }
        
        lineRenderer.SetPosition(0, transform.position);

        DebugExtension.DebugPoint(_touchStartPosWorldSpace, Color.cyan);
        Debug.DrawRay(_touchStartPosWorldSpace, _directionWorldSpace, _directionChosen ? Color.green : Color.red);
        
        DebugExtension.DebugPoint(targetJoint.target, Color.yellow);
    }

    private void RaycastAndMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxGrappleDistance, LayerMask.GetMask("Grapple"));

        if (hit)
        {
            Debug.DrawRay(transform.position, direction, Color.blue, 2f);
            DebugExtension.DebugPoint(hit.point, Color.magenta, 1f, 2f);

            float currentVelocityMagnitude = body.linearVelocity.magnitude;
            float projectedMagnitude = Vector2.Dot(body.linearVelocity, direction.normalized);
            body.linearVelocity = direction.normalized * ((currentVelocityMagnitude + projectedMagnitude) / 2f);
            
            targetJoint.enabled = true;
            targetJoint.target = hit.point;
            
            lineRenderer.SetPosition(1, hit.point);
        }
    }
}
