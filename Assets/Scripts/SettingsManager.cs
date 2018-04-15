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
    
	void OnEnable () {
        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antiAliasingDropdown.onValueChanged.AddListener(delegate { OnAntiAliasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        resolutions = Screen.resolutions;

	}
	
	public void OnFullscreenToggle()
    {
        Screen.fullscreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {

    }

    public void OnTextureQualityChange()
    {

    }

    public void OnAntiAliasingChange()
    {

    }

    public void OnVSyncChange()
    {

    }

    public void OnVolumeChange()
    {

    }

    public void SaveSettings()
    {

    }

    public void LoadSettings()
    {

    }
}
