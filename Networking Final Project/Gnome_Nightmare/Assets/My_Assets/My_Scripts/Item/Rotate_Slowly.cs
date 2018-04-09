using UnityEngine;

public class Rotate_Slowly : MonoBehaviour {

    //The speed the object will rotate at
    public Vector3 RotationSpeed = new Vector3(0.0f, 1.0f, 0.0f);
	
	// Update is called once per frame
	void Update () {
        //Will rotate the object this script is attached to
        this.transform.Rotate(RotationSpeed);
	}
}
