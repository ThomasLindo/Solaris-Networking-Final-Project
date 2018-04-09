using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random_Utils;

namespace EnemySpawners { 
    
    public class Spawner_Settings : SerializedMonoBehaviour {
        public static Spawner_Settings instance;
        void Awake() { instance = this; }
    
        //Where enemies are set to spawn
        public GameObject WorldEnenies;
    
        public Spawner spawner = new Spawner();
    
        // Use this for initialization
        private void Start () {
            if (WorldEnenies == null) { WorldEnenies = GameObject.Find("Enemies"); }
            if (spawner.spawnerDetails.SpawnerPosition == null) { spawner.spawnerDetails.SpawnerPosition = this.gameObject; }
        }
    
        // Update is called once per frame
        private void Update () {
            if (!spawner.Toggle) { spawner.spawnerDetails.UpdateSpawners(); }
        }
    
        public void SpawnGodDamnIt(SpawnerEnemies Prefab, int number, Vector3 randomPosition) {
            GameObject tempObj = Instantiate(Prefab.enemyPrefab, this.transform.position + randomPosition, this.transform.rotation);
            tempObj.transform.SetParent(WorldEnenies.transform);
            Prefab.IncreaseStats(tempObj);
            tempObj.name = Prefab.enemyPrefab.name;
        }
    
    
    }
    
    public class Spawner : ISpawnTables {
        //[TableColumnWidth(50)]
        [HorizontalGroup("Group 0", 0.1f), LabelWidth(50)]
        public bool Toggle;
    
        //[HorizontalGroup("Group 0", 0.3f), LabelWidth(100)]
        //public GameObject SpawnerPosition;
    
        [HorizontalGroup("Group 0", 0.0f), LabelWidth(50)]
        public int StartAt = 1;
        [HorizontalGroup("Group 0", 0.0f), LabelWidth(80)]
        public int ActiveEvery = 1;
        [HideInInspector]
        //[TableColumnWidth(1)]
        public int LastAvtiveRound = 0;
    
        public SpawnerDetails spawnerDetails = new SpawnerDetails();
    }
    public class SpawnerDetails : ISpawnTables {
    
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
        [HideInInspector]
        public GameObject SpawnerPosition;
    
        [System.NonSerialized]
        public int number = 0;
    
        [TableList]
        public List<SpawnerEnemies> spawnerEnemies = new List<SpawnerEnemies>();
    
        // Update is called once per frame
        public void UpdateSpawners() {
            if (DefeatedSpawner) { return; }
            spawnCoolDownRemaining -= Time.deltaTime;
            if (spawnCoolDownRemaining < 0) {
                spawnCoolDownRemaining = Spawn;
                SpawnMob();
            }
        }
    
        private void SpawnMob() {
            if (DefeatedSpawner) { return; }
    
            bool SpawnedAMob = false;
            // Go through all wave components until we find something to spawn
            foreach (SpawnerEnemies wc in spawnerEnemies) {
                if (wc.NumberSpawned < wc.Amount) {
                    Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-SpawnPosition.x, 0.0f, -SpawnPosition.z), new Vector3(SpawnPosition.x, SpawnPosition.y, SpawnPosition.z));
                    wc.NumberSpawned++;
                    //Transform tempTransform = EnemySpawnTable.instance.spawners[EnemySpawnTable.instance.spawnerDetails.IndexOf(this)].SpawnerPosition.transform;
                    //Spawner_Settings.instance.SpawnGodDamnIt(wc, number, RandomPosition);
                    GameObject tempObj = UnityEngine.GameObject.Instantiate(wc.enemyPrefab, SpawnerPosition.transform.position + RandomPosition, SpawnerPosition.transform.rotation);
                    tempObj.transform.SetParent(Spawner_Settings.instance.WorldEnenies.transform);
                    wc.IncreaseStats(tempObj);
                    tempObj.name = wc.enemyPrefab.name;
    
                    SpawnedAMob = true;
                    break;
                }
                else if (wc.NumberSpawned >= wc.Amount) { wc.EndOfWaveComp = true; }
            }
            if (spawnerEnemies[spawnerEnemies.Count - 1].EndOfWaveComp) {
                WaveNumber++;
                if (SpawnedAMob == false) {
                    if (WaveNumber < NumberOfWaves) {
                        foreach (SpawnerEnemies wc in spawnerEnemies) {
                            wc.NumberSpawned = 0;
                            wc.EndOfWaveComp = false;
                        }
                    }
                    else {
                        DefeatedSpawner = true;
                        spawnCoolDownRemaining = Round;
                        Interface_SpawnTable.instance.CheckAllSpawners();
                        foreach (SpawnerEnemies wc in spawnerEnemies) {
                            //wc.IncreaseStats();
                            wc.NumberSpawned = 0;
                            wc.EndOfWaveComp = false;
                        }
                    }
                }
            }
        }
        public void IncreaseStats() {
            foreach (SpawnerEnemies wc in spawnerEnemies) { wc.IncreaseStats(); }
        }
    
        public int TotalEnemyCount() {
            int totalEnemyCount = 0;
            foreach (SpawnerEnemies wc in spawnerEnemies) {
                totalEnemyCount += wc.Amount;
            }
            return totalEnemyCount;
        }
    
    }
    public class SpawnerEnemies : ISpawnTables {
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
