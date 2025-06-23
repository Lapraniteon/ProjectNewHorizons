using System;
using UnityEngine;
using DG.Tweening;

public class Grapple : MonoBehaviour
{

    // Object references
    private Camera _mainCamera;
    public Rigidbody2D body;
    public LineRenderer lineRenderer;

    // Grappling
    public TargetJoint2D targetJoint;
    public float maxGrappleDistance;
    private Vector2 _touchStartPos;
    private Vector2 _touchStartPosWorldSpace;
    private Vector2 _direction;
    private Vector2 _directionWorldSpace;
    private bool _directionChosen;
    private bool _beganTouch;

    private string _currentGrappleTag;

    // Time tweening
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
                    _beganTouch = true;
                    _touchStartPos = touch.position;
                    _touchStartPosWorldSpace = _mainCamera.ScreenToWorldPoint(new Vector3(_touchStartPos.x, _touchStartPos.y, cameraZOffset));
                    _directionChosen = false;
                    _timeTweener = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.1f, 0.5f).SetEase(Ease.OutCubic);
                    break;
                
                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    if (!_beganTouch)
                        break;
                    
                    Vector2 touchPositionWorldSpace =
                        _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cameraZOffset));
                    
                    _directionWorldSpace = touchPositionWorldSpace - _touchStartPosWorldSpace;

                    if (_directionWorldSpace.magnitude > 4f)
                    {
                        _beganTouch = false;
                        _directionChosen = true;
                    }
                    
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:

                    _timeTweener.Kill();
                    Time.timeScale = 1f;
                    
                    if (Vector2.Distance(_touchStartPos, touch.position) < 30f)
                    {
                        DisconnectGrapple();
                        break;
                    }
                    
                    _beganTouch = false;
                    _directionChosen = true;
                    break;
            }
        }
        
        // If direction is chosen, launch.
        if (_directionChosen)
        {
            Debug.Log("Launch!");

            lineRenderer.enabled = true;
            RaycastAndMove(_directionWorldSpace);
            _directionChosen = false;
        }
        
        // If grappling a vine, release grapple when in close proximity
        if (_currentGrappleTag == "Vine" && Vector2.Distance(transform.position, targetJoint.target) <= 1f)
            DisconnectGrapple();
        
        lineRenderer.SetPosition(0, transform.position);

        DebugExtension.DebugPoint(_touchStartPosWorldSpace, Color.cyan);
        Debug.DrawRay(_touchStartPosWorldSpace, _directionWorldSpace, _directionChosen ? Color.green : Color.red);
        
        DebugExtension.DebugPoint(targetJoint.target, Color.yellow);
        DebugExtension.DebugWireSphere(targetJoint.target, Color.red, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bounce"))
        {
            DisconnectGrapple();
            other.gameObject.GetComponent<BounceObject>().Bounce(body);
        }

        if (other.CompareTag("PsychoShroom"))
        {
            other.gameObject.GetComponent<PsychoShroom>().Trigger();
        }

        if (other.CompareTag("Death"))
        {
            Die();
        }
    }

    private void DisconnectGrapple()
    {
        targetJoint.enabled = false;
        lineRenderer.enabled = false;
    }

    private void RaycastAndMove(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxGrappleDistance, LayerMask.GetMask("Grapple", "Vine"));

        if (hit)
        {
            _currentGrappleTag = hit.transform.tag;
            
            Debug.DrawRay(transform.position, direction, Color.blue, 2f);
            DebugExtension.DebugPoint(hit.point, Color.magenta, 1f, 2f);

            float currentVelocityMagnitude = body.linearVelocity.magnitude;
            float projectedMagnitude = Vector2.Dot(body.linearVelocity, direction.normalized);
            //body.linearVelocity = direction.normalized * ((currentVelocityMagnitude + projectedMagnitude) / 2f);
            body.linearVelocity = direction.normalized * currentVelocityMagnitude;
            
            targetJoint.enabled = true;
            targetJoint.target = hit.point;
            
            lineRenderer.SetPosition(1, hit.point);
        }
    }

    public void Die()
    {
        GameManager.Instance.PlayerKilled();
    }
}
