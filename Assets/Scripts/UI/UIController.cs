using System;
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
    public TMP_Text[] highScoreTexts;
    public TMP_Text[] collectableTexts;

    [Space] 
    public TMP_Text endScreenStarterHighScoreText;
    public TMP_Text endScreenMushroomHighScoreText;
    public TMP_Text endScreenTotalHighScoreText;
    
    // Events
    [HorizontalLine]
    [Header("Events")]
    public UnityEvent onGamePause;
    public UnityEvent onGameResume;
    
    public UnityEvent onQuitToMainMenu;
    public UnityEvent onBackToMainMenu;
    public UnityEvent onBackToPauseScreen;
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
    
    public void BackToMainMenu()
    {
        onBackToMainMenu.Invoke();
    }
    
    public void BackToPauseScreen()
    {
        onBackToPauseScreen.Invoke();
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
        
        ingameTimerText.enabled = GameManager.Instance.ShowIngameTimer;
        
        onShowTimerButtonPressed.Invoke();
    }

    private void Start()
    {
        GameManager.Instance.uiController = this;
    }

    private void Update()
    {
        UpdateTimerDisplays();
        
        if (endScreenStarterHighScoreText != null)
            endScreenStarterHighScoreText.text = GameManager.Instance.GetStarterHighScore().ToString();
        
        if (endScreenMushroomHighScoreText != null)
            endScreenMushroomHighScoreText.text = GameManager.Instance.GetMushroomHighScore().ToString();
        
        if (endScreenTotalHighScoreText != null)
            endScreenTotalHighScoreText.text = GameManager.Instance.GetTotalHighScore().ToString();
    }

    public void UpdateCollectablesDisplays(int currentAmount, int totalAmount)
    {
        foreach (TMP_Text textObj in collectableTexts)
            textObj.text = $"{currentAmount}/{totalAmount}";
    }

    public void UpdateTimerDisplays()
    {
        float currentTime = GameManager.Instance.currentRunTime;
        string formattedTime = FormatTime(currentTime);
        
        foreach (TMP_Text textObj in timerTexts)
            textObj.text = formattedTime;
        
        ingameTimerText.text = formattedTime;
    }

    public void UpdateHighScoreDisplays()
    {
        float highScore = GameManager.Instance.GetCurrentHighScore();

        foreach (TMP_Text textObj in highScoreTexts)
            textObj.text = FormatTime(highScore);
    }
    
    private string FormatTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string format = time switch
        {
            < 10f => @"s\.ff",
            < 60f => @"ss\.ff",
            < 3600f => @"mm\:ss\.ff",
            _ => @"hh\:mm\:ss\.ff"
        };

        string[] runTimeTextParts = timeSpan.ToString(format).Split('.'); // + "<size=50%><i>." + timeSpan.ToString("@fff");
        string runTimeText = runTimeTextParts[0] + "<size=65%>." + runTimeTextParts[1];
        
        return runTimeText;
    }
    
}
