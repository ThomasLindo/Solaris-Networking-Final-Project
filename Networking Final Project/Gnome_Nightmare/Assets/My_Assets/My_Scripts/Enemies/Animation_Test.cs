using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Test : MonoBehaviour {

    public Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("1")) { anim.Play("Gnome_Hit", -1, 0f); }
        else if (Input.GetKeyDown("2")) { anim.Play("Gnome_Die", -1, 0f); }
    }
}
