using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Info : MonoBehaviour {
    private void Awake() { DontDestroyOnLoad(this.gameObject); }

    public SettingsMenu settingsMenu;

    public float Volume = -40.0f;
    public int qualityIndex = 2;
    public bool isFullscreen = false;
    public Resolution resolution;
    public int ResolutionIndex = 0;

    //private void Start() { InvokeRepeating("UpdateSlow", 5.0f, 2.0f); }
    //private void UpdateSlow() {
    //    if (GameObject.FindObjectOfType(typeof(SettingsMenu)) as SettingsMenu) {
    //        settingsMenu = GameObject.FindObjectOfType(typeof(SettingsMenu)) as SettingsMenu;
    //        if (settingsMenu.gameObject.name == "Canvas") { ApplyThis(); CancelInvoke(); }
    //    }
    //}

    public void ApplyThis() {
        settingsMenu.SetVolume(Volume);
        settingsMenu.SetQuality(qualityIndex);
        settingsMenu.SetFullscreen(isFullscreen);
        settingsMenu.ResolutionIndex = ResolutionIndex;
        settingsMenu.SetResolution(resolution);
        Debug.Log("Applied");
    }


}
