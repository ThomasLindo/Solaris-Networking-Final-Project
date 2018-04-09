using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInArea : MonoBehaviour {

    public int Amount = 0;
    public bool PlayersInArea = false;
    

    void OnTriggerEnter(Collider obj) {
        if (obj.tag == "Player") { PlayersInArea = true; Amount += 1; }
    }
    void OnTriggerExit(Collider obj) {
        if (obj.tag == "Player") {
            Amount -= 1;
            if (Amount <= 0) { PlayersInArea = false; }
        }
    }
}
