using UnityEngine;

//Based off of a youtube video
//https://www.youtube.com/watch?v=EgNV0PWVaS8&list=PLdnBuK2UbqBQnM2LZcraHyLGu3heJVO7F&index=5
//

public class Destructible : MonoBehaviour {

    public GameObject destroyedVersion;
    public float MaxVelocity = 100.0f;
    private bool Destroyed = false;

    //Used to destroy [this] gameObject and Instantiate the new destroyed version of it
    public void Kill() {
        //Instantiate the destroyed version
        GameObject v_destroyedVersion = Instantiate(destroyedVersion, this.transform.position, this.transform.rotation);
        //Destroys the destroyed version in 6 seconds
        Destroy(v_destroyedVersion, 6.0f);
        //Destroys [this] gameObject
        Destroy(this.gameObject);
        Destroyed = true;
    }

    //Used to destroy [this] gameObject if it collids at a high velocity and Instantiate the new destroyed version of it
    private void OnCollisionEnter(Collision collision) {
        //If the collision happend at a high velocity
        if (collision.relativeVelocity.magnitude > MaxVelocity && Destroyed == false) {
            //Instantiate the destroyed version
            GameObject v_destroyedVersion = Instantiate(destroyedVersion, this.transform.position, this.transform.rotation);
            //Destroys the destroyed version in 6 seconds
            Destroy(v_destroyedVersion, 6.0f);
            //Destroys [this] gameObject
            Destroy(this.gameObject);
            Destroyed = true;
        }
    }

    //Click to destroy
    private void OnMouseDown() {
        //Instantiate(destroyedVersion, this.transform.position, this.transform.rotation);
        //Destroy(this.gameObject);
        //Destroyed = true;
    }
}
