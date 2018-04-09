using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rotation : MonoBehaviour {

    public Transform bone; //Players Spine
    public Transform cameraTarget; //A target 10 meters infron of the camera
    public Transform HitBox; //Players Hitbox


    private void Start() {
        if (cameraTarget == null) { cameraTarget = GameObject.Find("Main Camera").transform.GetChild(0).transform; }
    }

    void LateUpdate() {
        bone.LookAt(cameraTarget); //Rotate the Players spine
        HitBox.LookAt(cameraTarget); //Rotate the Players hit box
    }
}
