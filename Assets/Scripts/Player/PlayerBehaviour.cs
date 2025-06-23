using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public Grapple grappleComponent;

    void Start()
    {
        GameManager.Instance.player = this;
        
        grappleComponent = GetComponent<Grapple>();
    }
    
    public void Die()
    {
        GameManager.Instance.PlayerKilled();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
            Die();
    }
}
