#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject highScoreTab;
    public GameObject settingTab;
   
    public void NewGame()
    {
        AudioManager.instance.ApplySetting();
        AudioManager.instance.PlaySFX(0);
        SceneManager.LoadScene(1);
    }
    
    public void HighScore()
    {
        HighScoreManager.instance.ShowHighScore();
        AudioManager.instance.ApplySetting();
        AudioManager.instance.PlaySFX(1);
        highScoreTab.SetActive(true);
        settingTab.SetActive(false);
    }

    public void Setting()
    {
        AudioManager.instance.ApplySetting();
        AudioManager.instance.PlaySFX(1);
        settingTab.SetActive(true);
        highScoreTab.SetActive(false);
    }

    public void Back()
    {
        AudioManager.instance.ApplySetting();
        AudioManager.instance.PlaySFX(1);
        highScoreTab.SetActive(false);
        settingTab.SetActive(false);
    }

    public void Exit()
    {
        AudioManager.instance.ApplySetting();
        AudioManager.instance.PlaySFX(1);
        Application.Quit();
    }

}
