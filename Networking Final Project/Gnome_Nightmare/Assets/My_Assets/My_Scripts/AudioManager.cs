using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

	// Use this for initialization
	void Awake () {

        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); return; }

        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < sounds.Length; i++) {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.outputAudioMixerGroup = sounds[i].output;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
            sounds[i].source.loop = sounds[i].loop;
        }
	}

    private void Start() {
        Play("Back Ground Sound");
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source == null) {
            Debug.Log("Sound couldn't be played.");
            return;
        }
        s.source.Play();
    }
}
