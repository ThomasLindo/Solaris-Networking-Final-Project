using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyObjectBoundaryColliderCheck : MonoBehaviour {

    //Used because [Entrance] has 2 colliders
    private int InCollider;

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == "Player") {
			if (transform.name == "Entrance") { Event_Manager.instance.playerInEntranceBoundary = true; InCollider++; }
			else if(transform.name == "Generator Room") { Event_Manager.instance.playerInGRBoundary = true; }
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.name == "Player") {
			if (transform.name == "Entrance") {
                InCollider--;
                if (InCollider <= 0) { Event_Manager.instance.playerInEntranceBoundary = false; }
            }
			else if(transform.name == "Generator Room") { Event_Manager.instance.playerInGRBoundary = false; }
		}
	}
}
