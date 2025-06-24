using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class SceneSwitcher : MonoBehaviour
{
    [Scene]
    public int sceneToLoad;

    public UnityEvent onLevelExit;

    public void SwitchScene()
    {
        onLevelExit.Invoke();

        SceneManager.LoadScene(sceneToLoad);
    }
}
