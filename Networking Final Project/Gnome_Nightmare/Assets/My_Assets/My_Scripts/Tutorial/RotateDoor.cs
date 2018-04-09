using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDoor : MonoBehaviour {

    public bool Activate = false;
    public bool IsOpen = false;

    public Vector3 RotationSpeed = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 DebugRot = new Vector3(0.0f, 0.0f, 0.0f);

    public Vector3 CloseRotation = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 OpenRotation = new Vector3(0.0f, 0.0f, 0.0f);


    // Update is called once per frame
    private void Update () {
        if (Activate) {
            //Will rotate the object this script is attached to
            if (IsOpen) {
                this.transform.Rotate(-RotationSpeed);
                DebugRot = this.transform.localRotation.eulerAngles;

                if (DebugRot.y != 0 && CloseRotation.y != 0) {
                    if (CloseRotation.y >= (DebugRot.y - Mathf.Abs(RotationSpeed.y)) && CloseRotation.y <= (DebugRot.y + Mathf.Abs(RotationSpeed.y))) {
                        this.transform.localRotation = Quaternion.Euler(CloseRotation);
                        Activate = false;
                        IsOpen = false;
                    }
                }
            }
            else {
                this.transform.Rotate(RotationSpeed);
                DebugRot = this.transform.localRotation.eulerAngles;

                if (DebugRot.y != 0 && OpenRotation.y != 0) {
                    if (OpenRotation.y >= (DebugRot.y - Mathf.Abs(RotationSpeed.y)) && OpenRotation.y <= (DebugRot.y + Mathf.Abs(RotationSpeed.y))) {
                        this.transform.localRotation = Quaternion.Euler(OpenRotation);
                        Activate = false;
                        IsOpen = true;
                    }
                }
            }
        }
	}
}
