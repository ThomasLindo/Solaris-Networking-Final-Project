using UnityEngine;

public class Orbit_Objects : MonoBehaviour {

    public float xSpread;
    public float zSpread;
    public float yOffset;
    public float yInitRotate;
    public Transform centerPoint;

    public float rotSpeed;
    public bool rotateClockwize;


    float timer = 0;

    // Use this for initialization
    void Start () {
        yInitRotate = yInitRotate / 60;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime * rotSpeed;
        Rotate();
        //OrbitAround();
        transform.LookAt(centerPoint);
    }

    //
    void Rotate() {
        if (rotateClockwize) {
            float x = -Mathf.Cos(timer + yInitRotate) * xSpread;
            float z = Mathf.Sin(timer + yInitRotate) * zSpread;
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + centerPoint.position;
        } else {
            float x = Mathf.Cos(timer + yInitRotate) * xSpread;
            float z = Mathf.Sin(timer + yInitRotate) * zSpread;
            Vector3 pos = new Vector3(x, yOffset, z);
            transform.position = pos + centerPoint.position;
        }
    }

    //
    void OrbitAround() {
        transform.RotateAround(centerPoint.position, Vector3.up, rotSpeed * Time.deltaTime);
    }

}
