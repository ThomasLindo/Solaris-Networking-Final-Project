using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour {

    public bool IsTriggered = false;

    private void OnTriggerStay(Collider other) {
        //Don't collide with
        //if (other.gameObject.layer == 2 || other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 12) { IsTriggered = false; }
        //Collide with
        if (other.gameObject.layer == 0 || other.gameObject.layer == 4 || other.gameObject.layer == 8 ) { IsTriggered = true; }
    }
    private void OnTriggerExit(Collider other) {
        //IsTriggered = false;
        if (other.gameObject.layer == 0 || other.gameObject.layer == 4 || other.gameObject.layer == 8) { IsTriggered = false; }
    }
}
