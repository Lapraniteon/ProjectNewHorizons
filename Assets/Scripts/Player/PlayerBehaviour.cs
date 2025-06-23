using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    

    public void Die()
    {
        GameManager.Instance.PlayerKilled();
    }
}
