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
    public bool GamePaused { get; private set; }

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
    
    // Timer
    public float currentRunTime;
    public bool runTimePaused;
    
    // Pausing
    public float currentTimeScale;

    void Awake()
    {
        _instance = this;
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        
        if (player == null)
            player = FindAnyObjectByType<PlayerBehaviour>();
    }

    void Update()
    {
        cameraTrackingTarget.SetPositionAndRotation(player.transform.position, player.transform.rotation);
        
        if (!runTimePaused)
            currentRunTime += Time.deltaTime;
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

    public void CollectableCollected() => uiController.UpdateCollectablesDisplays(currentCollectableController.AmountCollected, currentCollectableController.Amount);
    
    public void ResetRunTime() => currentRunTime = 0f;
    
    public void SetRunTimePaused(bool paused) => runTimePaused = paused;

    public void FinalizeAndSaveRunTime()
    {
        PlayerPrefs.SetFloat("RunTime", currentRunTime);
        Debug.Log("Saved runtime of " + currentRunTime);
    }

    public float GetCurrentHighScore()
    {
        return PlayerPrefs.GetFloat("HighScore", 0);
    }

    public void TogglePause()
    {
        if (!GamePaused)
        {
            currentTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            player.Pause();
            
            GamePaused = true;
        }
        else
        {
            Time.timeScale = currentTimeScale;
            
            player.Unpause();
            
            GamePaused = false;
        }
    }
}
