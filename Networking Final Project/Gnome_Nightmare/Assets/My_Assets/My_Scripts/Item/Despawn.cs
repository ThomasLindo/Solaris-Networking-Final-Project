using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour {

    public float StartIn = 0.0f;
    public float RepeatEvery = 1.0f;
    public float DespawnAfter = 60.0f;
    private float counter = 0.0f;

    // Use this for initialization
    void Start () { InvokeRepeating("SlowUpdate", StartIn, RepeatEvery); }
	
	// Update is called once per frame
	void SlowUpdate() {
        counter += 1;
        if (counter > DespawnAfter) { Destroy(this.gameObject); }
    }
}
