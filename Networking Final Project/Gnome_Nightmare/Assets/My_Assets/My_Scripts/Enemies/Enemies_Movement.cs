using UnityEngine;
using UnityEngine.AI;
using Random_Utils;

public class Enemies_Movement : MonoBehaviour {

    [SerializeField]
    Vector3 destination;
    NavMeshAgent navMeshAgent;
    GameObject[] players;
    public Animator anim;

    public float MaxFollowDistance = 100.0f;
    public float MaxAttackDistance = 1.0f;
    public float RangeDivisible = 10.0f;
    public float TimerDelay = 0.50f;
    public float TimerDelayModifier = 30.0f;
    public float AttackDelay = 0.90f;

    private float timer = 0.0f;
    private float AttackTimer = 0.0f;
    private float ClosestPlayerDistance;

    // Use this for initialization
    void Start () {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        Invoke("DelayedStart", 0.1f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        players = GameObject.FindGameObjectsWithTag("Player");
    }


    private void Update() {
        if (AttackTimer > 0.0f) { AttackTimer -= Time.deltaTime; }

        //Will only update enemies seeking position at certain intervals
        if (timer > 0.0f) { timer -= Time.deltaTime; }
        else {
            timer = TimerDelay;
            if (navMeshAgent == null) { return; }
            else { FindDestination(); }
        }
    }

    private void FindDestination() {
        //Makes sure there is players to seek
        if (players != null) {
            int playerNum = 0;
            //Goes through all players and checks their distance to the enemy
            playerNum = FindClosestPlayer();

            //If the player is withen Attack Distance
            if (ClosestPlayerDistance <= MaxAttackDistance && AttackTimer <= 0.0f) {
                float MoE = AttackDelay * 0.1f;
                AttackTimer = RandomUtils.RandomFloat(AttackDelay - MoE, AttackDelay + MoE); ;
                AttackPlayer(playerNum);
                navMeshAgent.SetDestination(this.transform.position);
                return;
            }
            //If the player is withen Follow Distance but outside Attack Distance
            else if (ClosestPlayerDistance < MaxFollowDistance) {
                TimerDelay = Mathf.Clamp((ClosestPlayerDistance / TimerDelayModifier), 0.1f, 1.0f);
                SetDestination(playerNum);
                return;
            }
            //The enemy will wonder
            else if (ClosestPlayerDistance > MaxAttackDistance) { Wonder(); return; }
        }
        else { return; }
    }

    private int FindClosestPlayer() {
        int playerNum = 0;
        ClosestPlayerDistance = MaxFollowDistance;
        //Goes through all players and checks their distance to the enemy
        for (int i = 0; i < players.Length; i++) {
            //Distance between player and enemy
            float tempDistance = Vector3.Distance(players[i].transform.position, this.transform.position);
            //Finds closes player
            if (tempDistance <= ClosestPlayerDistance && !players[i].GetComponent<PlayerStats>().isDead) {
                ClosestPlayerDistance = tempDistance;
                playerNum = i;
            }
        }
        return playerNum;
    }

    private void SetDestination(int playerNum) {
        //Random placement based on range
        float Range = (ClosestPlayerDistance / RangeDivisible);
        Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-Range, 0.0f, -Range), new Vector3(Range, 0.0f, Range));RandomPosition =
        destination = players[playerNum].transform.position;// + RandomPosition;
        //If there is a destination, set the navMeshAgent to go to it
        if (destination != null) { navMeshAgent.SetDestination(destination); }
    }

    private void AttackPlayer(int playerNum) {
        //Enemies will deal damage to the player
        players[playerNum].GetComponent<PlayerStats>().TakeDamage(this.gameObject.GetComponent<EnemyStats>().Damage.GetValue());
        if (anim != null) { anim.Play("Gnome_Hit", -1, 0f); }
    }

    private void Wonder() {
        //Enemies will move around randomly
        timer = RandomUtils.RandomFloat(1.5f, 4.0f);
        navMeshAgent.SetDestination(this.transform.position + RandomUtils.RandomVector3InBox(new Vector3(-10.0f, 1.0f, -10.0f),new Vector3(10.0f, 1.0f, 10.0f)));
    }

}
