using UnityEngine;
using UnityEngine.Events;

public class BounceObject : MonoBehaviour
{

    public Transform impulseVector;
    public float impulseStrength;

    public UnityEvent onBounce;
    
    public void Bounce(Rigidbody2D body)
    {
        body.MovePositionAndRotation(impulseVector.position, impulseVector.rotation);
        
        body.linearVelocity = Vector2.zero;
        body.AddForce(impulseVector.up * impulseStrength, ForceMode2D.Impulse);

        onBounce.Invoke();
    }
}
