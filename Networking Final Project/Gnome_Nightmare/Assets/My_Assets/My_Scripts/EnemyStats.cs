using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : CharacterStats {

    public float Experience = 1;
    public float Points = 1;
    public bool isDead = false;
    //public GameObject ParticlesOnDeath;
    public Transform Health_Bar;
    

    private void Awake() {
        if (Health_Bar == null) {
            Health_Bar = this.gameObject.transform.Find("HealthBar");
        }
    }

    private void Update() {
       HealthBar();
    }

    //Applys damage to the enemy 
    //if it kills it return true
    public bool DamageEnemy(float amount) {
        //Make sure the enemy has health
        if (CurrentHealth <= 0.0f) { return false; }
        //apply damage
        TakeDamage(amount);

        //Check to see if the enemy died
        if (CurrentHealth <= 0.0f) { return true; }
        else { return false; }
    }

    //On enemy death destory the enemy
    public void OnDeath() {
        if (isDead) { return; }
        else { isDead = true; }

        FindDropItem();
        //If the enemy is a Destructible object, destroy through destruction
        Destructible isDestructible = this.GetComponent<Destructible>();
        if (isDestructible != null) { isDestructible.Kill(); }
        //Else just destroy the gameObject
        else { Destroy(this.gameObject, 0.9f); }

        //Play Enemy death animation
        if (this.gameObject.GetComponent<Enemies_Movement>()) {
            if (this.gameObject.GetComponent<Enemies_Movement>().anim != null) { this.gameObject.GetComponent<Enemies_Movement>().anim.Play("Die", -1, 0f); }
            this.gameObject.GetComponent<Enemies_Movement>().enabled = false;
        }
        //Disable enemy movement
        if (this.gameObject.GetComponent<NavMeshAgent>())    { this.gameObject.GetComponent<NavMeshAgent>().enabled = false; }
        //Disable enemy collision
        if (this.gameObject.GetComponent<CapsuleCollider>()) { this.gameObject.GetComponent<CapsuleCollider>().enabled = false; }
        if (this.gameObject.GetComponent<BoxCollider>())     { this.gameObject.GetComponent<BoxCollider>().enabled = false; }
        if (this.gameObject.GetComponent<MeshCollider>())    { this.gameObject.GetComponent<MeshCollider>().enabled = false; }

        //Send Death Message 
        if (this.gameObject.GetComponent<Agent>()) { this.gameObject.GetComponent<Agent>().SendDestroy(); }

        //if (ParticlesOnDeath != null) {
        //    GameObject v_ParticlesOnDeath = Instantiate(ParticlesOnDeath, this.transform.position, this.transform.rotation);
        //    Destroy(v_ParticlesOnDeath, 3.0f);
        //}
    }
    
    private void HealthBar(){
        if (Health_Bar == null) { return; }
        if (CurrentHealth == MaxHealth) { return; }
        else if (!Health_Bar.gameObject.activeSelf) { Health_Bar.gameObject.SetActive(true); }

        float v_Health = CurrentHealth / MaxHealth;
        v_Health = Mathf.Clamp(v_Health, 0.0f, 1.0f);
        Vector3 Health = new Vector3(v_Health, 1.0f, 1.0f);
        Health_Bar.GetChild(0).GetComponent<RectTransform>().transform.localScale = Health;
        Health = new Vector3(1.0f-(v_Health), 1.0f, 1.0f);
        Health_Bar.GetChild(1).GetComponent<RectTransform>().transform.localScale = Health;
        
        //Make sure the enemy has health
        //if (CurrentHealth <= 0.0f) { OnDeath(); }
    }


    private void FindDropItem() {
        EnemyDropList dropList = GameObject.Find("World").transform.Find("Enemies").GetComponent<EnemyDropList>();
        if (dropList != null) {
            for (int i = 0; i < dropList.Enemies.Count; i++) {
                if (dropList.Enemies[i].Enemy.name == this.gameObject.name) {
                    List<int> ItemsToSpawn = dropList.SpawnRandormDrop(i);
                    if (ItemsToSpawn == null) { return; }
                    dropList.SpawnItem(i, ItemsToSpawn, this.gameObject.transform);
                }
            }
        }
    }
}
