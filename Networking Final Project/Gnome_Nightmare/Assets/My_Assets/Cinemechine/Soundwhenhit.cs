using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundwhenhit : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "CM vcam1" || other.gameObject.tag == "Cinemachine_Trigger") {
            Debug.Log("Hit");
            this.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "CM vcam1" || other.gameObject.tag == "Cinemachine_Trigger") {
            //Debug.Log("Hit");
            //this.gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
