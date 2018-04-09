using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinemechine_Camera_Trigger : MonoBehaviour {

    private bool Loaded = false;

    //private void Start() {
    //    Time.timeScale = 10.0f;
    //    Time.fixedDeltaTime = 0.02f * Time.timeScale;
    //}

    private void Update() {
        if (Input.GetKey(KeyCode.Escape) || Input.GetButton("Start")) {
            LoadScene();
        }
    }



    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "CM vcam1" || other.gameObject.tag == "Cinemachine_Trigger") {
            Debug.Log("Hit");
            if (!Loaded) { Invoke("LoadScene", 2.0f); Loaded = true; }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "CM vcam1" || other.gameObject.tag == "Cinemachine_Trigger") {
            Debug.Log("Hit");
            if (!Loaded) { Invoke("LoadScene", 2.0f); Loaded = true; }
        }
    }

    private void LoadScene() {
        SceneManager.LoadScene("Gnome_Nightmare");
    }

}
