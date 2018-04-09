using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Random_Utils;

namespace EnemySpawners {

    public class Spawner_Manager : SerializedMonoBehaviour {
        public static Spawner_Manager instance;
        void Awake() { instance = this; }
    
        public bool ToggleAll = false;
    
        public EnemySpawnTable enemySpawnTable = new EnemySpawnTable();
    
        // Use this for initialization
        private void Start () {
            for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
                if (enemySpawnTable.spawners[i].spawnersDetails != null) { 
                    enemySpawnTable.spawners[i].spawnersDetails.number = i;
                }
            }
        }
    
        private void Update() {
            if (enemySpawnTable != null) {
                for (int i = 0; i < enemySpawnTable.spawners.Count; i++) {
                    if (enemySpawnTable.spawners[i].spawnersDetails != null) {  
                        if (!ToggleAll && !enemySpawnTable.spawners[i].Toggle && enemySpawnTable.spawners[i].SpawnerPosition != null) {
                            enemySpawnTable.spawners[i].spawnersDetails.UpdateSpawners();
                        }
                    }
                }
            }
        }
    
        public void SpawnGodDamnIt(SpawnersEnemies Prefab, int number, Vector3 randomPosition) {
            GameObject tempObj = Instantiate(Prefab.enemyPrefab, enemySpawnTable.spawners[number].SpawnerPosition.transform.position + randomPosition, enemySpawnTable.spawners[number].SpawnerPosition.transform.rotation);
            tempObj.transform.SetParent(Spawner_Manager.instance.enemySpawnTable.WorldEnenies.transform);
            Prefab.IncreaseStats(tempObj);
            tempObj.name = Prefab.enemyPrefab.name;
        }
    }
    
    
    public interface ISpawnTables { }
    public class EnemySpawnTable : ISpawnTables {
        public static EnemySpawnTable instance;
        void Awake() { instance = this; }
        //Where enemies are set to spawn
        public GameObject WorldEnenies;
    
        //[TableList]
        //[TabGroup("Spawners", false, 0)]
        public List<Spawners> spawners = new List<Spawners>();
    }
    public class Spawners : ISpawnTables {
        //[TableColumnWidth(50)]
        [HorizontalGroup("Group 0", 0.1f), LabelWidth(50)]
        public bool Toggle;
    
        [HorizontalGroup("Group 0", 0.3f), LabelWidth(100)]
        public GameObject SpawnerPosition;
    
        [HorizontalGroup("Group 0", 0.0f), LabelWidth(50)]
        public int StartAt = 1;
        [HorizontalGroup("Group 0", 0.0f), LabelWidth(80)]
        public int ActiveEvery = 1;
        [HideInInspector]
        //[TableColumnWidth(1)]
        public int LastAvtiveRound = 0;
    
        public SpawnersDetails spawnersDetails = new SpawnersDetails();
    }
    public class SpawnersDetails : ISpawnTables {
    
        [HorizontalGroup("Group 1", 0.5f), LabelWidth(110)]
        public bool DefeatedSpawner = false;
        [HorizontalGroup("Group 1", 0.5f), LabelWidth(120)]
        public int NumberOfWaves = 1;
        private int WaveNumber = 0;
        [FoldoutGroup("Interval Between")]
        [HorizontalGroup("Interval Between/Split"), LabelWidth(50)]
        [MinValue(0), MaxValue(10)]
        public float Spawn = 1.25f;
        [HorizontalGroup("Interval Between/Split"), LabelWidth(50)]
        [MinValue(0), MaxValue(60)]
        public float Round = 5.0f;
        [System.NonSerialized]
        public float spawnCoolDownRemaining = 5.0f;
        [FoldoutGroup("Interval Between"), LabelWidth(100)]
        public Vector3 SpawnPosition = new Vector3(0, 0, 0);
    
        [System.NonSerialized]
        public int number = 0;
    
        [TableList]
        public List<SpawnersEnemies> spawnersEnemies = new List<SpawnersEnemies>();
    
        // Update is called once per frame
        public void UpdateSpawners() {
            if (DefeatedSpawner) { return; }
            spawnCoolDownRemaining -= Time.deltaTime;
            if (spawnCoolDownRemaining < 0) {
                spawnCoolDownRemaining = Spawn;
                SpawnMob();
            }
        }
    
        void SpawnMob() {
            if (DefeatedSpawner) { return; }
    
            bool SpawnedAMob = false;
            // Go through all wave components until we find something to spawn
            foreach (SpawnersEnemies wc in spawnersEnemies) {
                if (wc.NumberSpawned < wc.Amount) {
                    Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-SpawnPosition.x, 0.0f, -SpawnPosition.z), new Vector3(SpawnPosition.x, SpawnPosition.y, SpawnPosition.z));
                    wc.NumberSpawned++;
                    //Transform tempTransform = EnemySpawnTable.instance.spawners[EnemySpawnTable.instance.spawnerDetails.IndexOf(this)].SpawnerPosition.transform;
                    Spawner_Manager.instance.SpawnGodDamnIt(wc, number, RandomPosition);
                    SpawnedAMob = true;
                    break;
                }
                else if (wc.NumberSpawned >= wc.Amount) { wc.EndOfWaveComp = true; }
            }
            if (spawnersEnemies[spawnersEnemies.Count - 1].EndOfWaveComp) {
                WaveNumber++;
                if (SpawnedAMob == false) {
                    if (WaveNumber < NumberOfWaves) {
                        foreach (SpawnersEnemies wc in spawnersEnemies) {
                            wc.NumberSpawned = 0;
                            wc.EndOfWaveComp = false;
                        }
                    }
                    else {
                        DefeatedSpawner = true;
                        spawnCoolDownRemaining = Round;
                        Interface_SpawnTable.instance.CheckAllSpawners();
                        foreach (SpawnersEnemies wc in spawnersEnemies) {
                            //wc.IncreaseStats();
                            wc.NumberSpawned = 0;
                            wc.EndOfWaveComp = false;
                        }
                    }
                }
            }
        }
        public void IncreaseStats() {
            foreach (SpawnersEnemies wc in spawnersEnemies) { wc.IncreaseStats(); }
        }
    
        public int TotalEnemyCount() {
            int totalEnemyCount = 0;
            foreach (SpawnersEnemies wc in spawnersEnemies) {
                totalEnemyCount += wc.Amount;
            }
            return totalEnemyCount;
        }
    
    }
    public class SpawnersEnemies : ISpawnTables {
        [TableColumnWidth(260)]
        [VerticalGroup("Enemies"), LabelWidth(80)]
        public GameObject enemyPrefab;
        [VerticalGroup("Enemies"), LabelWidth(80)]
        public int Amount;
    
        [TableColumnWidth(90)]
        [VerticalGroup("Increase By"), LabelWidth(40)]
        public int Count;
        [VerticalGroup("Increase By"), LabelWidth(40)]
        public int Health;
        [VerticalGroup("Increase By"), LabelWidth(40)]
        public int Points;
    
        [TableColumnWidth(100)]
        [VerticalGroup("Increase Stats"), LabelWidth(50)]
        public int Armour;
        [VerticalGroup("Increase Stats"), LabelWidth(50)]
        public int Damage;
        [VerticalGroup("Increase Stats"), LabelWidth(50)]
        public int Exp;
    
        //[HorizontalGroup("Group 2", 0.8f), LabelWidth(90)]
        [System.NonSerialized]
        public int NumberSpawned = 0;
        [System.NonSerialized]
        public bool EndOfWaveComp = false;
    
    
        //private int IncreasedCount;
        private int IncreasedHealth;
        private int IncreasedPoints;
        private int IncreasedArmour;
        private int IncreasedDamage;
        private int IncreasedExp;
    
        public void IncreaseStats() {
            Amount += Count;
            IncreasedHealth += Health;
            IncreasedArmour += Armour;
            IncreasedDamage += Damage;
            IncreasedExp += Exp;
            IncreasedPoints += Points;
        }
        public void IncreaseStats(GameObject ThisEnemy) {
            ThisEnemy.GetComponent<EnemyStats>().MaxHealth += IncreasedHealth;
            ThisEnemy.GetComponent<EnemyStats>().Armour.baseValue += IncreasedArmour;
            ThisEnemy.GetComponent<EnemyStats>().Damage.baseValue += IncreasedDamage;
            ThisEnemy.GetComponent<EnemyStats>().Experience += IncreasedExp;
            ThisEnemy.GetComponent<EnemyStats>().Points += IncreasedPoints;
        }
    }
}