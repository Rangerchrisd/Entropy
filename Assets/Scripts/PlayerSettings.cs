using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public enum languageEnum{
    none,
    English,
    Japanese
    }

public class PlayerSettings : MonoBehaviour
{
    public GameObject settingsMenu;
    public float musicVolume;
    public float masterVolume;
    public float gameSpeed;
    public float CameraSpeed;

    public AudioMixer soundMixer;

    public Slider musicSlider;
    public Slider masterSoundSlider;
    public Slider gameSpeedSlider;
    public Slider cameraSpeeddSlider;
    public Dropdown dropdown;

    void Start()
    {
        if (FindObjectsOfType<PlayerSettings>().Length != 1)
        {
            Destroy(this.gameObject);
        }
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        masterSoundSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        gameSpeedSlider.value = PlayerPrefs.GetFloat("GameSpeed", 1f);
        //dropdown.value =  PlayerPrefs.GetInt("Language", (int)languageEnum.English);
        DontDestroyOnLoad(this.gameObject);
    }


    public void MusicSetLevel(float sliderValue)
    {
        soundMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        if (sliderValue == 0) {
            soundMixer.SetFloat("MusicVol", 0);
        }
        musicVolume = Mathf.Log10(sliderValue) * 20;
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void MasterSetLevel(float sliderValue)
    {
        soundMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        if (sliderValue == 0)
        {
            soundMixer.SetFloat("MusicVol", 0);
        }
        masterVolume = Mathf.Log10(sliderValue) * 20;
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
    public void gameSpeedSetLevel(float sliderValue)
    {
        //set to .1+slidervalue
        gameSpeed = sliderValue;
        PlayerPrefs.SetFloat("GameSpeed", sliderValue);
    }
    public void cameraSpeedSetLevel(float sliderValue)
    {
        CameraSpeed = sliderValue;
        //set to .1+slidervalue
        PlayerPrefs.SetFloat("CameraSpeed", sliderValue);
    }
}
