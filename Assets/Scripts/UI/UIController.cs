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
    public TMP_Text endScreenTotalRunTimeText;
    
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
        Time.timeScale = 1f;
        
        onStartGame.Invoke();
        
        SceneManager.LoadScene(gameScene);
    }

    public void ToggleIngameTimer(bool enabled)
    {
        GameManager.Instance.SetShowIngameTimer(enabled);
        
        if (ingameTimerText != null)
            ingameTimerText.enabled = PlayerPrefs.GetInt("ShowIngameTimer", 1) == 1 ? true : false;
        
        onShowTimerButtonPressed.Invoke();
    }

    private void Start()
    {
        GameManager.Instance.uiController = this;
        
        if (ingameTimerText != null)
            ingameTimerText.enabled = PlayerPrefs.GetInt("ShowIngameTimer", 1) == 1 ? true : false;
    }

    private void Update()
    {
        UpdateTimerDisplays();
        
        if (endScreenStarterHighScoreText != null)
            endScreenStarterHighScoreText.text = FormatTime(GameManager.Instance.GetStarterHighScore());
        
        if (endScreenMushroomHighScoreText != null)
            endScreenMushroomHighScoreText.text = FormatTime(GameManager.Instance.GetMushroomHighScore());
        
        if (endScreenTotalHighScoreText != null)
            endScreenTotalHighScoreText.text = FormatTime(GameManager.Instance.GetTotalHighScore());
        
        if (endScreenTotalRunTimeText != null)
            endScreenTotalRunTimeText.text = FormatTime(PlayerPrefs.GetFloat("CurrentRunTime", 0));
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
        
        if (ingameTimerText != null)
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
            //< 10f => @"s\.ff",
            //< 60f => @"ss\.ff",
            < 3600f => @"mm\:ss\.ff",
            _ => @"hh\:mm\:ss\.ff"
        };

        string[] runTimeTextParts = timeSpan.ToString(format).Split('.'); // + "<size=50%><i>." + timeSpan.ToString("@fff");
        string runTimeText = runTimeTextParts[0] + "<size=65%>." + runTimeTextParts[1];
        
        return runTimeText;
    }
    
}
