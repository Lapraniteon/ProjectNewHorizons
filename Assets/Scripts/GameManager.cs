using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // Game Manager singleton pattern
    public static GameManager Instance
    {
        get
        {
            if (_instance is null) // Error checking in case the Game Manager is not assigned
                Debug.LogError("GameManager is null!");

            return _instance;
        }
    } // Game Manager instance property

    // Static player data
    public PlayerBehaviour player;
    public RespawnPoint currentRespawnPoint;
    
    public UnityEvent onPlayerDeath;

    void Awake()
    {
        _instance = this;
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }

    public void PlayerKilled()
    {
        onPlayerDeath.Invoke();

        player.grappleComponent.DisconnectGrapple();
        transform.SetPositionAndRotation(currentRespawnPoint.transform.position, currentRespawnPoint.transform.rotation);
    }
}
