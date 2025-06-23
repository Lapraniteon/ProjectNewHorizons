using NaughtyAttributes;
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
    
    // Global flags
    public bool overlayActive;
    public bool ShowIngameTimer { get; private set; }

    // Static player data
    [ShowAssetPreview]
    public PlayerBehaviour playerPrefab;
    public PlayerBehaviour player;
    public RespawnPoint currentRespawnPoint;
    public Transform cameraTrackingTarget;
    
    public UnityEvent onPlayerDeath;
    
    // Collectables
    [HorizontalLine] 
    public CollectableController currentCollectableController;
    
    // UI
    public UIController uiController;

    void Awake()
    {
        _instance = this;
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }

    void Update()
    {
        cameraTrackingTarget.SetPositionAndRotation(player.transform.position, player.transform.rotation);
    }

    public void PlayerKilled()
    {
        onPlayerDeath.Invoke();

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Destroy(player.gameObject);
        player = Instantiate(playerPrefab, currentRespawnPoint.transform.position, Quaternion.identity);
        Debug.Log("Player respawned");
    }

    public void SetRespawnPoint(RespawnPoint respawnPoint)
    {
        currentRespawnPoint = respawnPoint;
        Debug.Log("New respawn point set at " + currentRespawnPoint.transform.position);
    }
    
    public void ToggleShowIngameTimer() => ShowIngameTimer = !ShowIngameTimer;
    public void SetShowIngameTimer(bool showIngameTimer) => ShowIngameTimer = showIngameTimer;
}
