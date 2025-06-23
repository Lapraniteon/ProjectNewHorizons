using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{

    private float _previousTimeScale;

    [Header("Scene References")]
    [Scene] public int mainMenuScene;
    [Scene] public int gameScene;

    // Text references
    [HorizontalLine]
    public TMP_Text ingameTimerText;
    public TMP_Text[] timerTexts;
    public TMP_Text[] collectableTexts;
    
    // Events
    [HorizontalLine]
    [Header("Events")]
    public UnityEvent onGamePause;
    public UnityEvent onGameResume;
    
    public UnityEvent onQuitToMainMenu;
    public UnityEvent onOptionsButtonPressed;
    public UnityEvent onLevelSelectButtonPressed;
    public UnityEvent onShowTimerButtonPressed;
    
    public UnityEvent onReloadCurrentScene;
    
    public UnityEvent onStartGame;
    public UnityEvent onQuitGame;
    
    // Misc
    private Canvas _currentCanvas;

    public void Pause()
    {
        _previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        
        onGamePause.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = _previousTimeScale;

        onGameResume.Invoke();
    }

    public void QuitToMainMenu()
    {
        onQuitToMainMenu.Invoke();
        
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Options()
    {
        onOptionsButtonPressed.Invoke();
    }

    public void ReloadCurrentScene()
    {
        onReloadCurrentScene.Invoke();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelSelect(string sceneName)
    {
        onLevelSelectButtonPressed.Invoke();
        
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        onQuitGame.Invoke();
        
        Application.Quit();
    }

    public void StartGame()
    {
        onStartGame.Invoke();
        
        SceneManager.LoadScene(gameScene);
    }

    public void ToggleIngameTimer()
    {
        GameManager.Instance.ToggleShowIngameTimer();
        
        onShowTimerButtonPressed.Invoke();
    }

    private void Start()
    {
        GameManager.Instance.uiController = this;
    }
    
}
