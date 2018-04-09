using UnityEngine;

public class Eyes_Follow_Cursor : MonoBehaviour {
    public bool BasedOnCamera = true;
    public bool BasedOnObject = false;
    public Transform Object;

    void Update () {
        if (BasedOnCamera) { this.transform.rotation = Camera.main.transform.rotation; }
        else if (BasedOnObject) { this.transform.rotation = Object.rotation; }
    }
}
