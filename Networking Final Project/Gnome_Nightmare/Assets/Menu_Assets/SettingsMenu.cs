using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    [HideInInspector]
    public int ResolutionIndex = 0;
    public Resolution resolution;
    public bool isFullscreen = true;
    public Slider VolumeSlider;
    public Dropdown GraphicsDropdown;

    private Setting_Info setting_Info;

    private void Awake() {
        setting_Info = GameObject.FindObjectOfType(typeof(Setting_Info)) as Setting_Info;
        if (!VolumeSlider) { VolumeSlider = GameObject.Find("Volume Slider").GetComponent<Slider>(); }
        if (!GraphicsDropdown) { GraphicsDropdown = GameObject.Find("Graphics Dropdown").GetComponent<Dropdown>(); }
        if (!resolutionDropdown) { resolutionDropdown = GameObject.Find("Resolution Dropdown").GetComponent<Dropdown>(); }

        if (setting_Info) {
            setting_Info.settingsMenu = this;
            setting_Info.ApplyThis();
        }
    }

    private void Start() {
        resolutions = Screen.resolutions;
        List<Resolution> resolutionsV2 = new List<Resolution>();


        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        int j = 0;

        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (i >= 1 && option != (resolutions[i-1].width + " x " + resolutions[i-1].height)) {
                j++;
                resolutionsV2.Add(resolutions[i]);
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) { currentResolutionIndex = j; }
            }
        }

        resolutions = resolutionsV2.ToArray();

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }


    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
        if (setting_Info) { setting_Info.Volume = volume; }
        if (VolumeSlider) { VolumeSlider.value = volume; }
    }

    public void ToggleTimer(bool isOn) {

    }
    public void SetTimer(float volume) {

    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
        if (setting_Info) { setting_Info.qualityIndex = qualityIndex; }
        if (GraphicsDropdown) {
            GraphicsDropdown.value = qualityIndex;
            GraphicsDropdown.RefreshShownValue();
        }
    }

    public void ToggleFullscreen() {
        if (isFullscreen) { isFullscreen = false; }
        else { isFullscreen = true; }
        
        Screen.fullScreen = isFullscreen;
        if (setting_Info) { setting_Info.isFullscreen = isFullscreen; }
    }
    public void SetFullscreen(bool _isFullscreen) {
        isFullscreen = _isFullscreen;
        Screen.fullScreen = _isFullscreen;
        if (setting_Info) { setting_Info.isFullscreen = _isFullscreen; }
    }

    public void SetResolution(int resolutionIndex) {
        resolution = resolutions[resolutionIndex];
        ResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        if (setting_Info) { setting_Info.resolution = resolution; }
        if (resolutionDropdown) {
            resolutionDropdown.value = resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }
    public void SetResolution(Resolution r) {
        resolution = r;
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        if (setting_Info) { setting_Info.ResolutionIndex = ResolutionIndex; }
        if (resolutionDropdown) {
            resolutionDropdown.value = ResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }
    public void SetResolution() {
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        if (setting_Info) { setting_Info.ResolutionIndex = ResolutionIndex; }
        if (resolutionDropdown) {
            resolutionDropdown.value = ResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

}
