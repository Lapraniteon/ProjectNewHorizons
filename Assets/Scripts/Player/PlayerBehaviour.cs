using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Grapple grappleComponent;
    private Rigidbody2D _rigidbody;

    private Vector2 _linearVelocityBeforePause;
    private float _angularVelocityBeforePause;

    void Start()
    {
        GameManager.Instance.player = this;
        
        grappleComponent = GetComponent<Grapple>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void Die()
    {
        GameManager.Instance.PlayerKilled();
    }

    public void Pause()
    {
        _linearVelocityBeforePause = _rigidbody.linearVelocity;
        _angularVelocityBeforePause = _rigidbody.angularVelocity;
        
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Unpause()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        grappleComponent.DisconnectGrapple();
        
        _rigidbody.linearVelocity = _linearVelocityBeforePause;
        _rigidbody.angularVelocity = _angularVelocityBeforePause;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
            Die();

        if (other.CompareTag("RespawnPoint"))
            GameManager.Instance.SetRespawnPoint(other.GetComponent<RespawnPoint>());

        if (other.CompareTag("Collectable"))
        {
            other.GetComponent<Collectable>().Collect();
        }

        if (other.CompareTag("SceneSwitcher"))
        {
            other.GetComponent<SceneSwitcher>().SwitchScene();
        }
    }
}
