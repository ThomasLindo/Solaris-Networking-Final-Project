using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour {
    public bool doOneEighty = false; // will the ui need to rotate 180?

	// Update is called once per frame
	void Update () {        
       GetComponent<RectTransform>().LookAt(Camera.main.transform.position);

        if (doOneEighty) {
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
