using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLayer : MonoBehaviour {

    public int LayerOnEnter; //Walk through
    public int LayerOnExit; //Collision
    public GameObject Wall;

    private void OnTriggerEnter(Collider other) {
        Debug.Log(LayerOnEnter);
        if (other.gameObject.tag == "Player") { Wall.layer = LayerOnEnter; }
    }
    private void OnTriggerExit(Collider other) {
        Debug.Log(LayerOnExit);
        if (other.gameObject.tag == "Player") { Wall.layer = LayerOnExit; }
    }
}
