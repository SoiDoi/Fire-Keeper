using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int enemyKilled;
    public TimeManager timeManager;
    public int aliveDay;
    [Header("UI")]
    public GameObject gameOverMenu;
    public GameObject toolBarUI;
    public GameObject statusUI;
    public GameObject dayUI;
    public GameObject gamePauseMenu;
    public GameObject settingUI;
    public bool isGameOver = false;

    public TMP_Text aliveText;
    public TMP_Text killText;
    public string playerName;
    private void Awake()
    {
        AudioManager.instance.PlayMusic(1);
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        aliveDay = timeManager.currentDay;
        toolBarUI.SetActive(false);
        statusUI.SetActive(false);
        dayUI.SetActive(false);
        gameOverMenu.SetActive(true);
        isGameOver = true; 
        Time.timeScale = 0f;
        aliveText.text = "Alive Day: " + aliveDay.ToString();
        killText.text = "Enemies Killed: "+ enemyKilled.ToString();
    }

    public void UpdateName(string name)
    {
        playerName = name;
    }
    public void SaveScore()
    {
        Debug.Log("Save");
        HighScoreManager.instance.AddHighScore(playerName ==null ? "Noname" : playerName, aliveDay, enemyKilled);
    }
    
    public void GamePause()
    {
        toolBarUI.SetActive(false);
        statusUI.SetActive(false);
        dayUI.SetActive(false);
        gamePauseMenu.SetActive(true);
        AudioManager.instance.PlaySFX(1);
        Time.timeScale = 0f;
    }

    public void GameContinue()
    {
        toolBarUI.SetActive(true);
        statusUI.SetActive(true);
        dayUI.SetActive(true);
        gamePauseMenu.SetActive(false);
        AudioManager.instance.PlaySFX(1);
        Time.timeScale = 1f;
    }

    public void Retry()
    {
        SaveScore();
        AudioManager.instance.PlaySFX(1);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        if (isGameOver)
        {
            SaveScore();
        }
        Time.timeScale = 1f;
        AudioManager.instance.PlaySFX(1);
        AudioManager.instance.PlayMusic(0);
        SceneManager.LoadScene(0);

    }

    public void Setting()
    {
        AudioManager.instance.LoadAudioSetting();
        gamePauseMenu.SetActive(false );
        settingUI.SetActive(true);
    }

    public void BackSetting()
    {
        gamePauseMenu.SetActive(true);
        settingUI.SetActive(false);
    }

}
