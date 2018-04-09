using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMeshRenderer : MonoBehaviour {

    public Material DepthMask;
    public Material Default;
    public bool On = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") { this.gameObject.GetComponent<MeshRenderer>().material = DepthMask; On = true; }
    }
    private void OnTriggerStay(Collider other) {
        if (On == false && other.gameObject.tag == "Player") { this.gameObject.GetComponent<MeshRenderer>().material = DepthMask; On = true; }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") { this.gameObject.GetComponent<MeshRenderer>().material = Default; On = false; }
    }
}
