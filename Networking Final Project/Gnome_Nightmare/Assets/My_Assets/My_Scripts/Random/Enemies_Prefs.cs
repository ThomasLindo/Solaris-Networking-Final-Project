using UnityEngine;

public class Enemies_Prefs : MonoBehaviour {

    Transform targetPathNode;

    public GameObject Goal_1;
    public GameObject Goal_2;

    public float speed = 5.0f;
    public float health = 1.0f;
    public int moneyValue = 1;

    // Use this for initialization
    void Start() { }

    // Find closest goal for enemies
    void closestGoal() {
        float distance_1 = Vector3.Distance(this.transform.position, Goal_1.transform.position);
        float distance_2 = Vector3.Distance(this.transform.position, Goal_2.transform.position);

        if (distance_1 <= distance_2) { targetPathNode = Goal_1.transform; }
        else { targetPathNode = Goal_2.transform; }
    }

    // Enemies collid with a goal
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Goal") {
            ReachedGoal();
        }
    }

    // Update is called once per frame
    void Update() {
        closestGoal();

        Vector3 dir = targetPathNode.position - this.transform.localPosition;

        float distThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame) {
            // We reached the node
            targetPathNode = null;
        } else {
            // TODO: Consider ways to smooth this motion.

            // Move towards node
            transform.Translate(dir.normalized * distThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 3);
        }

    }

    void ReachedGoal() {
        //GameObject.FindObjectOfType<ScoreManager>().LoseLife();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die() {
        // TODO: Do this more safely!
        //GameObject.FindObjectOfType<ScoreManager>().money += moneyValue;
        Destroy(gameObject);
    }
}
