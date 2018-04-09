using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownHub : MonoBehaviour {
    
    public float Multiplier = 1.0f; //Amount the player will be slowed by

    // Use this for initialization
    void Start () {
        foreach (SlowDown comp in gameObject.GetComponentsInChildren<SlowDown>()) {
            comp.Multiplier = Multiplier;
        }
    }
}
