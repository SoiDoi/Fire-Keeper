using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager instance;
    public List<HighScore> highScoreList = new List<HighScore>();
    public string savePath;
    public GameObject HighScoreObj;
    public Transform HighScoreTransform;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            savePath = Application.persistentDataPath + "/highscores.json";
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddHighScore(string name, int days, int kills)
    {
        HighScore entry = new HighScore
        {
            playerName = name,
            daysSurvived = days,
            enemiesKilled = kills
        };

        highScoreList.Add(entry);
        highScoreList.Sort((a, b) => b.daysSurvived.CompareTo(a.daysSurvived));

        SaveHighScore();
    }

    private void SaveHighScore()
    {
        string json = JsonUtility.ToJson(new HighscoreListWrapper { list = highScoreList }, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            HighscoreListWrapper wrapper = JsonUtility.FromJson<HighscoreListWrapper>(json);
            highScoreList = wrapper.list ?? new List<HighScore>();
        }
    }

    public void ShowHighScore()
    {
        foreach (Transform child in HighScoreTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Debug.Log(HighScoreTransform.childCount);
        foreach (var entry in HighScoreManager.instance.highScoreList)
        {
            if (HighScoreTransform.childCount >5) break;
            GameObject highScore = Instantiate(HighScoreObj,Vector3.zero, Quaternion.identity);
            highScore.transform.SetParent(HighScoreTransform);
            highScore.transform.GetChild(0).GetComponent<TMP_Text>().text = entry.playerName;
            highScore.transform.GetChild(1).GetComponent<TMP_Text>().text = entry.daysSurvived.ToString();
            highScore.transform.GetChild(2).GetComponent<TMP_Text>().text = entry.enemiesKilled.ToString();
            Debug.Log($"T¨ºn: {entry.playerName} - Ng¨¤y: {entry.daysSurvived} - Kill: {entry.enemiesKilled}");
        }
    }
    [System.Serializable]
    private class HighscoreListWrapper
    {
        public List<HighScore> list;
    }
}
