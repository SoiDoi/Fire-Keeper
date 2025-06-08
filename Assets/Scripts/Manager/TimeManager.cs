using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    public Light2D globalLight;

    public float currentTime = 0f;
    public float timePerDay = 144f;  //1440f; // 1 day = 60s
    public int currentDay = 1;

    public TMP_Text dayText;
    public TMP_Text hourText;

    void Start()
    {
        currentTime = 10;
       
    }

    private void Update()
    {
        DayCycle();
    }

    public void DayCycle()
    {
        currentTime += Time.deltaTime;
        if (currentTime > timePerDay)
        {
            currentDay++;
            currentTime = 0;
            SpawnManager.instance.StopSpawning();
            Debug.Log("stop spawn");
        }
        if (currentTime <= ((2 * timePerDay) / 3))
        {
            globalLight.intensity = Mathf.Lerp(1f, 0f, currentTime / ((2 * timePerDay) / 3));
        }

        if (currentTime > ((2 * timePerDay) / 3))
        {
            globalLight.intensity = Mathf.Lerp(0f, 1f, (currentTime - ((2 * timePerDay) / 3)) / (timePerDay - ((2 * timePerDay) / 3)));
            SpawnManager.instance.StartSpawning(); // spawn enemy
            Debug.Log("spawn");
        }

        float hourShow = 0;
        if ((currentTime / 6 + 6) < 24) //60
        {
            hourShow = currentTime / 6 + 6;
        }
        else
        {
            hourShow = (currentTime / 6) - 18;
        }
        if ((int)hourShow == 5) {AudioManager.instance.PlayMusic(1); Debug.Log("S"); };
        if ((int)hourShow == 21) {AudioManager.instance.PlayMusic(2); }
        dayText.text = "Day: " + currentDay;
        hourText.text = "Hour: " + (int)hourShow + "h";

    }

}
