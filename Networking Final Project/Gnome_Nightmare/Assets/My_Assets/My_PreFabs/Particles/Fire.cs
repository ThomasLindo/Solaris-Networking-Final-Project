using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public float Damage = 1.0f; //Amount of damage that will be applyed to the player
	public float health = 10.0f;

	private void Update() {
		if (health < 1.0f) {
			Event_Manager.fireCount--;
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision col) {
		if(col.gameObject.GetComponent<Bullet_Benaviour>() && col.gameObject.GetComponent<Bullet_Benaviour>().TypeOfAmmo == Ammo_Types.Ammo.Extinguisher 
            && !Event_Manager.getEventStatus(0, 1) && !Event_Manager.fireFailed) { health--; }
	}
    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.GetComponent<Bullet_Benaviour>() && col.gameObject.GetComponent<Bullet_Benaviour>().TypeOfAmmo == Ammo_Types.Ammo.Extinguisher 
            && !Event_Manager.getEventStatus(0, 1) && !Event_Manager.fireFailed) { health--; }
    }

    private void OnCollisionStay(Collision collision)  {
        //If the player collides with the Fire, apply damage.
        if (collision.gameObject.tag == "Player") { collision.gameObject.GetComponent<PlayerStats>().TakeDamage(Damage); }
        else if (collision.gameObject.name == "HitBox" && collision.gameObject.transform.parent.gameObject.tag == "Player") {
            collision.gameObject.transform.parent.gameObject.GetComponent<PlayerStats>().TakeDamage(Damage);
        }
    }
    private void OnTriggerStay(Collider other) {
        //If the player collides with the Fire, apply damage.
        if (other.gameObject.tag == "Player") { other.gameObject.GetComponent<PlayerStats>().TakeDamage(Damage); }
        else if (other.gameObject.name == "HitBox" && other.gameObject.transform.parent.gameObject.tag == "Player") {
            other.gameObject.transform.parent.gameObject.GetComponent<PlayerStats>().TakeDamage(Damage);
        }
    }
}
