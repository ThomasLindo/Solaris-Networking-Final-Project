using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWhenHit : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Destroy(this.gameObject);
        }
    }
}
