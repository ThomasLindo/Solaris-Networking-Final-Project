using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamageInfo : MonoBehaviour {

    private Melee_Behaviour melee_Behaviour;

    private void Awake() {
        if (melee_Behaviour == null && this.transform.parent.GetComponent<Melee_Behaviour>()) { melee_Behaviour = this.transform.parent.GetComponent<Melee_Behaviour>(); }
        else if (melee_Behaviour == null && this.transform.parent.parent.GetComponent<Melee_Behaviour>()) { melee_Behaviour = this.transform.parent.parent.GetComponent<Melee_Behaviour>(); }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.tag == "Enemy") {
            EnemyStats EnemyStat = other.transform.GetComponent<EnemyStats>();
            if (other.collider.GetComponent<EnemyStats>().DamageEnemy(melee_Behaviour.Stats.Damage.GetValue() + melee_Behaviour.playerManager.GetComponent<PlayerStats>().Damage.GetValue())) {
                melee_Behaviour.playerManager.GetComponent<PlayerStats>().AddExperience(EnemyStat.Experience);
                melee_Behaviour.playerManager.GetComponent<PlayerStats>().AddPoints(EnemyStat.Points);
                melee_Behaviour.playerManager.GetComponent<PlayerStats>().AddKills(1);
                //Destroys the enemy
                EnemyStat.OnDeath();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            EnemyStats EnemyStat = other.transform.GetComponent<EnemyStats>();
            if (other.GetComponent<EnemyStats>().DamageEnemy(melee_Behaviour.Stats.Damage.GetValue() + melee_Behaviour.playerManager.GetComponent<PlayerStats>().Damage.GetValue())) {
                melee_Behaviour.playerManager.GetComponent<PlayerStats>().AddExperience(EnemyStat.Experience);
                melee_Behaviour.playerManager.GetComponent<PlayerStats>().AddPoints(EnemyStat.Points);
                melee_Behaviour.playerManager.GetComponent<PlayerStats>().AddKills(1);
                //Destroys the enemy
                EnemyStat.OnDeath();
            }
        }
    }
}
