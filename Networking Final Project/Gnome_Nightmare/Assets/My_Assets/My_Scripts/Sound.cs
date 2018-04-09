
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public struct Sound {

    public string name;
    public AudioClip clip;
    public AudioMixerGroup output;
    [Range(0.0f, 2.0f)]
    public float volume;
    [Range(0.1f, 3.0f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
