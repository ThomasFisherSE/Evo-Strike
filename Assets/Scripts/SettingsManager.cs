using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {
    public Toggle fullscreenToggle;
    public Dropdown textureQualityDropdown;
    public Dropdown resolutionDropdown;
    public Dropdown antiAliasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider volumeSlider;
    
    public Resolution[] resolutions;
    public GameSettings gameSettings;

    public AudioSource audioSrc;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVol");
    }

    void OnEnable () {
        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antiAliasingDropdown.onValueChanged.AddListener(delegate { OnAntiAliasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        resolutions = Screen.resolutions;

        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
	}
	
	public void OnFullscreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    public void OnAntiAliasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antiAliasing = (int) Mathf.Pow(2, antiAliasingDropdown.value);
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    public void OnVolumeChange()
    {
        audioSrc.volume = gameSettings.globalVolume = volumeSlider.value;
        PlayerPrefs.SetFloat("MasterVol", audioSrc.volume);
        PlayerPrefs.Save();
    }

    public void SaveSettings()
    {

    }

    public void LoadSettings()
    {

    }
}
