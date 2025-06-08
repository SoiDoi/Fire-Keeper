using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip[] musicClip;
    public AudioClip[] sfxClip;

    [Range(0f, 1f)] public float musicVolume;
    [Range(0f, 1f)] public float sfxVolume;

    public Slider musicSlider;
    public Slider sfxSlider;

    public Toggle musicToggle;
    public Toggle sfxToggle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadAudioSetting();
        PlayMusic(0);
        ApplySetting();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMusic(int index)
    {
        if (index > musicClip.Length)
        {
            return;
        }
        musicSource.clip = musicClip[index];
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(int index)
    {
        if (index > sfxClip.Length)
        {
            return;
        }
        sfxSource.PlayOneShot(sfxClip[index]);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume > 0)
        {
            ToggleMusic(true);
            musicToggle.isOn = true;
        }
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        if (volume > 0)
        {
            ToggleSFX(true);
            sfxToggle.isOn = true;
        }
            sfxVolume = volume;
        sfxSource.volume = sfxVolume;
    }

    public void ApplySetting()
    {
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
        SaveAudioSetting();
    }

    public void ToggleMusic(bool isOn)
    {
        if (!isOn)
        {
            SetMusicVolume(0f);
            musicSlider.value = 0f;
        }
    }
    
    public void ToggleSFX(bool isOn)
    {
        if (!isOn)
        {
            SetSFXVolume(0f);
            sfxSlider.value = 0f;
        }
    }


    public void SaveAudioSetting()
    {
        PlayerPrefs.SetFloat("musicVolume",musicVolume);
        PlayerPrefs.SetFloat("sfxVolume",sfxVolume);
        PlayerPrefs.Save();
    }

    public void LoadAudioSetting()
    {
        float saveMusicVolume = PlayerPrefs.GetFloat("musicVolume");
        float saveSfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        musicSlider.value= saveMusicVolume;
        sfxSlider.value= saveSfxVolume;
    }

}
