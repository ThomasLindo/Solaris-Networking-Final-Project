using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DidPlayerCollide : MonoBehaviour {

    public bool IsTriggered = false;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" || other.tag == "player") { IsTriggered = true; }
    }
    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player" || other.tag == "player") { IsTriggered = true; }
    }
    private void OnTriggerExit(Collider other) {
        IsTriggered = false;
    }
}
