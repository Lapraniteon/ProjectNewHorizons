using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private Grapple _grappleComponent;
    private Rigidbody2D _rigidbody;

    private Vector2 _linearVelocityBeforePause;
    private float _angularVelocityBeforePause;

    void Start()
    {
        GameManager.Instance.player = this;
        
        _grappleComponent = GetComponent<Grapple>();
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
        _grappleComponent.DisconnectGrapple();
        
        _rigidbody.linearVelocity = _linearVelocityBeforePause;
        _rigidbody.angularVelocity = _angularVelocityBeforePause;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
            Die();

        if (other.CompareTag("RespawnPoint"))
        {
            RespawnPoint respawnPoint = other.GetComponent<RespawnPoint>();
            GameManager.Instance.SetRespawnPoint(respawnPoint);
            respawnPoint.onSetRespawn.Invoke();
        }
            

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
