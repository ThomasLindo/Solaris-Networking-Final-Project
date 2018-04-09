using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap : MonoBehaviour{

    public Transform player;
    public float CameraHight = 20.0f;
    public bool Rotate = false;

    private void Start() {
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate() {
        if (player == null) { return; }
        //Set position and rotation of the camera to above the player
        this.transform.position = new Vector3 (player.position.x, player.position.y + CameraHight, player.position.z);
        if (Rotate) {
            this.transform.rotation = Quaternion.Euler(90.0f, player.eulerAngles.y, 0.0f);
        }
    }
}
