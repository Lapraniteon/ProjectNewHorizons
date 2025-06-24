using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class SceneSwitcher : MonoBehaviour
{

    public bool loadScene = true;
    
    [Scene]
    public int sceneToLoad;

    public UnityEvent onLevelExit;

    public void SwitchScene()
    {
        onLevelExit.Invoke();

        if (loadScene)
            SceneManager.LoadScene(sceneToLoad);
    }
}
