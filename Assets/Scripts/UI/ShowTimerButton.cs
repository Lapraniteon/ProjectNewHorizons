using UnityEngine;

public class ShowTimerButton : MonoBehaviour
{
    public bool isOnButton;
    public bool ingameTimerEnabled;

    void Start()
    {
        ingameTimerEnabled = PlayerPrefs.GetInt("ShowIngameTimer", 1) == 1;
        Debug.Log(ingameTimerEnabled);

        if (isOnButton)
        {
            gameObject.SetActive(ingameTimerEnabled);
        }
        else
        {
            gameObject.SetActive(!ingameTimerEnabled);
        }
    }
}
