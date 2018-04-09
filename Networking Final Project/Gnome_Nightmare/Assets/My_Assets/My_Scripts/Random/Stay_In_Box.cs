using UnityEngine;

public class Stay_In_Box : MonoBehaviour {

    public GameObject BoundingBox;
    [Range(1.0f, 1000.0f)]
    public float SeekSpeed = 50.0f;

    private bool InBox = true;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        CheckIfInside();
    }

    //
    void CheckIfInside() {
        OnTriggerStay(BoundingBox.GetComponent<Collider>());
        if (!InBox) { transform.position = Vector3.MoveTowards(transform.position, BoundingBox.transform.position, SeekSpeed); }
    }

    //
    private void OnTriggerExit(Collider other) {
        //transform.position = Vector3.MoveTowards(transform.position, BoundingBox.transform.position, SeekSpeed);
    }

    //
    private void OnTriggerEnter(Collider other) {
        //do nothing
    }
    
    //
    private void OnTriggerStay(Collider other) {
        InBox = true;
    }

}
