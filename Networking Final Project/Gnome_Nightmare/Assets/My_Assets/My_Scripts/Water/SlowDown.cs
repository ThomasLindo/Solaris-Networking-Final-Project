using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour {

	public float Multiplier = 1.0f; //Amount the player will be slowed by
    

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Player") { collision.gameObject.GetComponent<Player_Movement>().SpeedModifier = Multiplier; }
        else if (collision.gameObject.name == "HitBox" && collision.gameObject.transform.parent.gameObject.tag == "Player") {
            collision.gameObject.transform.parent.gameObject.GetComponent<Player_Movement>().SpeedModifier = Multiplier;
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") { other.gameObject.GetComponent<Player_Movement>().SpeedModifier = Multiplier; }
        else if (other.gameObject.name == "HitBox" && other.gameObject.transform.parent.gameObject.tag == "Player") {
            other.gameObject.transform.parent.gameObject.GetComponent<Player_Movement>().SpeedModifier = Multiplier;
        }
    }


    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "Player") { collision.gameObject.GetComponent<Player_Movement>().SpeedModifier = 1.0f; }
        else if (collision.gameObject.name == "HitBox" && collision.gameObject.transform.parent.gameObject.tag == "Player") {
            collision.gameObject.transform.parent.gameObject.GetComponent<Player_Movement>().SpeedModifier = Multiplier;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") { other.gameObject.GetComponent<Player_Movement>().SpeedModifier = 1.0f; }
        else if (other.gameObject.name == "HitBox" && other.gameObject.transform.parent.gameObject.tag == "Player") {
            other.gameObject.transform.parent.gameObject.GetComponent<Player_Movement>().SpeedModifier = Multiplier;
        }
    }
}
