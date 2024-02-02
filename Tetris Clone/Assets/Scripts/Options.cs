using static UnityEditor.Progress;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Threading;
using UnityEditor;

public class Options : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;

    [SerializeField] private Toggle muteToggle;

    [SerializeField] private AudioSource movementSource;
    [SerializeField] private AudioSource clearLineSource;
    [SerializeField] private AudioSource bgmSource;

    private void Awake()
    {
        LoadSettings();
    }

    private void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution
        { width = resolution.width, height = resolution.height }).Distinct().ToArray(); ;

        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    private void Update()
    {
        //Register Slider Events
        musicVolume.onValueChanged.AddListener(delegate { changeMusicVolume(musicVolume.value); });
        sfxVolume.onValueChanged.AddListener(delegate { changesfxVolume(sfxVolume.value); });
    }

    private void LoadSettings()
    {
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bool muted = PlayerPrefs.GetInt("Muted", 0) == 1;

        Screen.fullScreen = fullscreen;
        fullscreenToggle.isOn = fullscreen;

        changeMusicVolume(musicVol);
        bgmSource.volume = musicVol;
        musicVolume.value = musicVol;

        changesfxVolume(sfxVol);
        movementSource.volume = sfxVol;
        clearLineSource.volume = sfxVol;
        sfxVolume.value = sfxVol;

        muteToggle.isOn = muted;
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen()
    {
        if (fullscreenToggle.isOn == true)
        {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("Fullscreen", 0);
        }        
    }

    //Called when Slider is moved
    private void changeMusicVolume(float sliderValue)
    {
        bgmSource.volume = sliderValue;
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    private void changesfxVolume(float sliderValue)
    {
        movementSource.volume = sliderValue;
        clearLineSource.volume = sliderValue;
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void SetMute()
    {
        if (muteToggle.isOn == true)
        {
            movementSource.mute = true;
            clearLineSource.mute = true;
            bgmSource.mute = true;

            musicVolume.interactable = false;
            sfxVolume.interactable = false;

            PlayerPrefs.SetInt("Muted", 1);
        }
        else
        {
            movementSource.mute = false;
            clearLineSource.mute = false;
            bgmSource.mute = false;

            musicVolume.interactable = true;
            sfxVolume.interactable = true;

            PlayerPrefs.SetInt("Muted", 0);
        }
    }
}
