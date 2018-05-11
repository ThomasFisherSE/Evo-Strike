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
    public Dropdown fitnessFuncDropdown;
    public Slider volumeSlider;
    
    public Resolution[] resolutions;
    public GameSettings gameSettings;

    public AudioSource audioSrc;

    /// <summary>
    /// Initialize the values of options UI components.
    /// Sets up listeners for UI components.
    /// </summary>
    void OnEnable () {
        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antiAliasingDropdown.onValueChanged.AddListener(delegate { OnAntiAliasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });
        fitnessFuncDropdown.onValueChanged.AddListener(delegate { OnFitnessFuncChange(); });
        resolutions = Screen.resolutions;

        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        volumeSlider.value = PlayerPrefs.GetFloat("MasterVol");
        fitnessFuncDropdown.value = (int)PlayerPrefs.GetFloat("FitnessFunc");

        fullscreenToggle.isOn = Screen.fullScreen;
        textureQualityDropdown.value = QualitySettings.masterTextureLimit;
        antiAliasingDropdown.value = QualitySettings.antiAliasing;
        vSyncDropdown.value = QualitySettings.vSyncCount;
    }
	
    public void OnFitnessFuncChange()
    {
        gameSettings.fitnessFunc = fitnessFuncDropdown.value;
        PlayerPrefs.SetFloat("FitnessFunc", gameSettings.fitnessFunc);
        PlayerPrefs.Save();
    }

	/// <summary>
    /// Toggle fullscreen mode.
    /// </summary>
    public void OnFullscreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    /// <summary>
    /// Change the screen resolution to the chosen option.
    /// </summary>
    public void OnResolutionChange()
    {
        gameSettings.resolutionIndex = resolutionDropdown.value;
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
    }

    /// <summary>
    /// Change the texture quality to the chosen option.
    /// </summary>
    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
    }

    /// <summary>
    /// Change the anti-aliasing method to the chosen option.
    /// </summary>
    public void OnAntiAliasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antiAliasing = (int) Mathf.Pow(2, antiAliasingDropdown.value);
    }

    /// <summary>
    /// Change the V-Sync method to the chosen option.
    /// </summary>
    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }

    /// <summary>
    /// Change and save the volume to the chosen value.
    /// </summary>
    public void OnVolumeChange()
    {
        audioSrc.volume = gameSettings.globalVolume = volumeSlider.value;
        PlayerPrefs.SetFloat("MasterVol", audioSrc.volume);
        PlayerPrefs.Save();
    }
}
