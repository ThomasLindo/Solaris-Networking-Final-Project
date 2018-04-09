using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Random_Utils;

namespace EnemySpawners { 
    
    public class Spawner_Hub : SerializedMonoBehaviour {

        [System.NonSerialized]
        public bool Toggle = false; // does not do anything

        [BoxGroup("Spawner Data", true, true)]
        [HorizontalGroup("Spawner Data/Group 0", 0.4f), LabelWidth(90)]
        public GameObject WorldEnenies;
        [HorizontalGroup("Spawner Data/Group 0", 0.6f), LabelWidth(110)]
        public Vector3 SpawnPosRange = new Vector3(0, 0, 0);

        [System.NonSerialized]
        public bool isOff = true;
        [HorizontalGroup("Spawner Data/Group 1", 0.33f), LabelWidth(100)]
        public float SpawnInterval = 2.0f;
        [HorizontalGroup("Spawner Data/Group 1", 0.33f), LabelWidth(70)]
        public float Increment = 1.0f;
        [HorizontalGroup("Spawner Data/Group 1", 0.33f), HideLabel]
        public float CapMin = 1.0f;

        [HorizontalGroup("Spawner Data/Group 2", 0.33f), LabelWidth(100)]
        public float RoundInterval = 10.0f;
        [HorizontalGroup("Spawner Data/Group 2", 0.33f), LabelWidth(70)]
        public float CoolDown = 1.25f;


        private int NumberOfWavesCompleted = 0;
        private int TotalSpawned = 0;
        private int IDStartAt = 100;

        [BoxGroup("Player Check", true, true)]
        [HorizontalGroup("Player Check/Barn", 0.02f), HideLabel]
        public bool SpawnAtBarn = true;
        [HorizontalGroup("Player Check/House", 0.02f), HideLabel]
        public bool SpawnAtHouse = true;
        [HorizontalGroup("Player Check/Bunker", 0.02f), HideLabel]
        public bool SpawnAtBunker = true;
        [HorizontalGroup("Player Check/Barn", 0.9f), LabelWidth(70)]
        public PlayerInArea BarnArea;
        [HorizontalGroup("Player Check/House", 0.9f), LabelWidth(70)]
        public PlayerInArea HouseArea;
        [HorizontalGroup("Player Check/Bunker", 0.9f), LabelWidth(70)]
        public PlayerInArea BunkerArea;

        //public List<GameObject> SpawnPositions = new List<GameObject>();
        [TabGroup("Enemies", "Spawn", false, 0)]
        public List<Enemies> EnemiesToSpawn = new List<Enemies>(1);
        [TabGroup("Enemies","Barn", false, 1)]
        public List<GameObject> Barn_SP = new List<GameObject>();
        [TabGroup("Enemies", "House", false, 2)]
        public List<GameObject> House_SP = new List<GameObject>();
        [TabGroup("Enemies", "Bunker", false, 3)]
        public List<GameObject> Bunker_SP = new List<GameObject>();


        public void Update() {
            if (isOff) { return; }

            if (BarnArea.PlayersInArea) { SpawnAtBarn = true; }
            else { SpawnAtBarn = false; }
            if (HouseArea.PlayersInArea) { SpawnAtHouse = true; }
            else { SpawnAtHouse = false; }
            if (BunkerArea.PlayersInArea) { SpawnAtBunker = true; }
            else { SpawnAtBunker = false; }

            CoolDown -= Time.deltaTime;
            if (CoolDown < 0) {
                CoolDown = SpawnInterval;
                SpawnMob();
            }

        }

        void SpawnMob() {
            if (isOff) { return; }
            // Go through all wave components until we find something to spawn
            foreach (Enemies et in EnemiesToSpawn) {

                if (et.NumberSpawned < (et.StartingAmount + et.IncreasedAmount) && et.isOff == false) {
                    

                    Vector3 RandomPosition = RandomUtils.RandomVector3InBox(new Vector3(-SpawnPosRange.x, 0.0f, -SpawnPosRange.z), new Vector3(SpawnPosRange.x, SpawnPosRange.y, SpawnPosRange.z));

                    int at = RandomUtils.RandomInt(1, 3);

                    if (SpawnAtBarn && at == 1) {
                        et.NumberSpawned++;
                        int number = RandomUtils.RandomInt(0, Barn_SP.Count - 1);
                        float speed = RandomUtils.RandomFloat(et.SpeedRange.x, et.SpeedRange.y);

                        GameObject tempObj = Instantiate(et.Enemy, Barn_SP[number].transform.position + RandomPosition, Barn_SP[number].transform.rotation);
                        tempObj.transform.SetParent(WorldEnenies.transform);
                        et.IncreaseStats(tempObj.GetComponent<EnemyStats>());
                        tempObj.name = et.Enemy.name;
                        tempObj.GetComponent<NavMeshAgent>().speed = speed;

                        Agent tempAgent = tempObj.GetComponent<Agent>();
                        tempAgent.AgentNumber = (TotalSpawned + IDStartAt);
                        tempAgent.Position = true;
                        tempAgent.Rotation = true;
                        tempAgent.Health = true;
                        tempAgent.SendInstantiate();
                        TotalSpawned += 1;

                        break;
                    }
                    if (SpawnAtHouse && at == 2) {
                        et.NumberSpawned++;
                        int number = RandomUtils.RandomInt(0, House_SP.Count-1);
                        float speed = RandomUtils.RandomFloat(et.SpeedRange.x, et.SpeedRange.y);

                        GameObject tempObj = Instantiate(et.Enemy, House_SP[number].transform.position + RandomPosition, House_SP[number].transform.rotation);
                        tempObj.transform.SetParent(WorldEnenies.transform);
                        et.IncreaseStats(tempObj.GetComponent<EnemyStats>());
                        tempObj.name = et.Enemy.name;
                        tempObj.GetComponent<NavMeshAgent>().speed = speed;

                        Agent tempAgent = tempObj.GetComponent<Agent>();
                        tempAgent.AgentNumber = (TotalSpawned + IDStartAt);
                        tempAgent.Position = true;
                        tempAgent.Rotation = true;
                        tempAgent.Health = true;
                        tempAgent.SendInstantiate();
                        TotalSpawned += 1;

                        break;
                    }
                    if (SpawnAtBunker && at == 3) {
                        et.NumberSpawned++;
                        int number = RandomUtils.RandomInt(0, Bunker_SP.Count-1);
                        float speed = RandomUtils.RandomFloat(et.SpeedRange.x, et.SpeedRange.y);

                        GameObject tempObj = Instantiate(et.Enemy, Bunker_SP[number].transform.position + RandomPosition, Bunker_SP[number].transform.rotation);
                        tempObj.transform.SetParent(WorldEnenies.transform);
                        et.IncreaseStats(tempObj.GetComponent<EnemyStats>());
                        tempObj.name = et.Enemy.name;
                        tempObj.GetComponent<NavMeshAgent>().speed = speed;

                        Agent tempAgent = tempObj.GetComponent<Agent>();
                        tempAgent.AgentNumber = (TotalSpawned + IDStartAt);
                        tempAgent.Position = true;
                        tempAgent.Rotation = true;
                        tempAgent.Health = true;
                        tempAgent.SendInstantiate();
                        TotalSpawned += 1;

                        break;
                    }
                    
                    if (!SpawnAtBarn && !SpawnAtHouse && !SpawnAtBunker) {
                        SpawnAtBarn = true;
                        SpawnAtHouse = true;
                        SpawnAtBunker = true;
                    }
                    break;
                }
                else if ((et.NumberSpawned >= (et.StartingAmount + et.IncreasedAmount) && et.isOff == false) || (et.NumberSpawned == 0 && et.isOff == true)) {
                    et.isOff = true;
                    et.EndOfWaveComp = true;
                    NumberOfWavesCompleted++;
                }
            }
           

            if (NumberOfWavesCompleted >= EnemiesToSpawn.Count) {
                NumberOfWavesCompleted = 0;
                foreach (Enemies et in EnemiesToSpawn) {
                    //Debug.Log("[" + et.NumberSpawned + "] [" + et.StartingAmount + "][" + et.IncreasedAmount + "]" + et.Enemy.name.ToString());
                    et.NumberSpawned = 0;
                    et.EndOfWaveComp = false;
                }
                TotalSpawned = 0;
                isOff = true;
                CoolDown = RoundInterval;
                //Make the Gnomes spawn Faster
                if (CapMin < SpawnInterval) {
                    SpawnInterval -= Increment;
                    if (CapMin > SpawnInterval) { SpawnInterval = CapMin; }
                }
                
                Interface_SpawnTable.instance.CheckAllSpawners();

            }
        }


        public void IncreaseAllStats() {
            foreach (Enemies et in EnemiesToSpawn) { et.IncreaseStats(); }
        }
        public void IncreaseRoundStats(int RoundNumber) {
            foreach (Enemies et in EnemiesToSpawn) {
                if (et.LastAvtiveRound == RoundNumber) { et.IncreaseStats(); }
            }
        }

        public int TotalEnemyCount() {
            int totalEnemyCount = 0;
            foreach (Enemies et in EnemiesToSpawn) {
                totalEnemyCount += (int)(et.StartingAmount + et.IncreasedAmount);
            }
            return totalEnemyCount;
        }

    }
    
    //public interface ISpawnTables { }
    public class Enemies : ISpawnTables{

        public Enemies() { }
        [System.NonSerialized]
        public bool isOff = true;


        [TableColumnWidth(100)]
        [HorizontalGroup("Enemies", 0.6f), LabelWidth(50)]
        public GameObject Enemy;
        [HorizontalGroup("Enemies", 0.4f), LabelWidth(100)]
        public int StartingAmount;

        [HorizontalGroup("Active", 0.5f), LabelWidth(50)]
        public int StartAt = 1;
        [HorizontalGroup("Active", 0.5f), LabelWidth(80)]
        public int ActiveEvery = 1;
        [System.NonSerialized]
        public int LastAvtiveRound = 0;

        [HorizontalGroup("Misc", 0.20f), LabelWidth(80)]
        public bool RandomIncrease = false;
        private bool NotRandomIncrease() { return !this.RandomIncrease; }
        private bool IsInEditMode() { return !Application.isPlaying; }
        
        [HorizontalGroup("Misc", 0.80f), MinMaxSlider(1, 20, true), LabelWidth(90)]
        public Vector2 SpeedRange;

        [System.NonSerialized]
        public int NumberSpawned = 0;
        [System.NonSerialized]
        public bool EndOfWaveComp = false;


        [ShowIf("RandomIncrease")]
        public EnemiesStatsRange IncreaseByRange = new EnemiesStatsRange();
        [ShowIf("NotRandomIncrease")]
        public EnemiesStatsInt IncreaseByInt = new EnemiesStatsInt();

        [System.NonSerialized]
        public  float IncreasedAmount;
        private float IncreasedHealth;
        private float IncreasedArmour;
        private float IncreasedDamage;
        private float IncreasedExp;
        private float IncreasedPoints;
    
        public void IncreaseStats() {
            if (RandomIncrease) {
                float IN = RandomUtils.RandomFloat(IncreaseByRange.Amount.x, IncreaseByRange.Amount.y);
                float IH = RandomUtils.RandomFloat(IncreaseByRange.Health.x, IncreaseByRange.Health.y);
                float IA = RandomUtils.RandomFloat(IncreaseByRange.Armour.x, IncreaseByRange.Armour.y);
                float ID = RandomUtils.RandomFloat(IncreaseByRange.Damage.x, IncreaseByRange.Damage.y);
                float IE = RandomUtils.RandomFloat(IncreaseByRange.Exp   .x, IncreaseByRange.Exp   .y);
                float IP = RandomUtils.RandomFloat(IncreaseByRange.Points.x, IncreaseByRange.Points.y);
                if (IncreaseByRange.Cap1 > IncreasedAmount) {
                    IncreasedAmount += IN;
                    if (IncreaseByRange.Cap1 < IncreasedAmount) { IncreasedAmount = IncreaseByRange.Cap1; }
                }
                if (IncreaseByRange.Cap2 > IncreasedHealth) {
                    IncreasedHealth += IH;
                    if (IncreaseByRange.Cap2 < IncreasedHealth) { IncreasedHealth = IncreaseByRange.Cap2; }
                }
                if (IncreaseByRange.Cap3 > IncreasedArmour) {
                    IncreasedArmour += IA;
                    if (IncreaseByRange.Cap3 < IncreasedArmour) { IncreasedArmour = IncreaseByRange.Cap3; }
                }
                if (IncreaseByRange.Cap4 > IncreasedDamage) {
                    IncreasedDamage += ID;
                    if (IncreaseByRange.Cap4 < IncreasedDamage) { IncreasedDamage = IncreaseByRange.Cap4; }
                }
                if (IncreaseByRange.Cap5 > IncreasedExp) {
                    IncreasedExp += IE;
                    if (IncreaseByRange.Cap5 < IncreasedExp) { IncreasedExp = IncreaseByRange.Cap5; }
                }
                if (IncreaseByRange.Cap6 > IncreasedPoints) {
                    IncreasedPoints += IP;
                    if (IncreaseByRange.Cap6 < IncreasedPoints) { IncreasedPoints = IncreaseByRange.Cap6; }
                }
            }
            else {
                if (IncreaseByInt.Cap1 > IncreasedAmount) {
                    IncreasedAmount += IncreaseByInt.Amount;
                    if (IncreaseByInt.Cap1 < IncreasedAmount) { IncreasedAmount = IncreaseByInt.Cap1; }
                }
                if (IncreaseByInt.Cap2 > IncreasedHealth) {
                    IncreasedHealth += IncreaseByInt.Health;
                    if (IncreaseByInt.Cap2 < IncreasedHealth) { IncreasedHealth = IncreaseByInt.Cap2; }
                }
                if (IncreaseByInt.Cap3 > IncreasedArmour) {
                    IncreasedArmour += IncreaseByInt.Armour;
                    if (IncreaseByInt.Cap3 < IncreasedArmour) { IncreasedArmour = IncreaseByInt.Cap3; }
                }
                if (IncreaseByInt.Cap4 > IncreasedDamage) {
                    IncreasedDamage += IncreaseByInt.Damage;
                    if (IncreaseByInt.Cap4 < IncreasedDamage) { IncreasedDamage = IncreaseByInt.Cap4; }
                }
                if (IncreaseByInt.Cap5 > IncreasedExp) {
                    IncreasedExp += IncreaseByInt.Exp;
                    if (IncreaseByInt.Cap5 < IncreasedExp) { IncreasedExp = IncreaseByInt.Cap5; }
                }
                if (IncreaseByInt.Cap6 > IncreasedPoints) {
                    IncreasedPoints += IncreaseByInt.Points;
                    if (IncreaseByInt.Cap6 < IncreasedPoints) { IncreasedPoints = IncreaseByInt.Cap6; }
                }
            }
        }
        public void IncreaseStats(EnemyStats ThisEnemy) {
            ThisEnemy.MaxHealth += IncreasedHealth;
            ThisEnemy.Armour.baseValue += IncreasedArmour;
            ThisEnemy.Damage.baseValue += IncreasedDamage;
            ThisEnemy.Experience += IncreasedExp;
            ThisEnemy.Points += IncreasedPoints;
        }
    }
    
    public class EnemiesStatsInt : ISpawnTables {
        public EnemiesStatsInt() { }
    
        [HorizontalGroup("Split", 250)]
    
        [TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 1"), LabelWidth(50)]
        public float Amount;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50)]
        public float Health;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50)]
        public float Points;

        [HorizontalGroup("Split", 20)]

        [TableColumnWidth(10)]
        [VerticalGroup("Split/Stats 2"), HideLabel, LabelWidth(10)]
        public int Cap1;
        [VerticalGroup("Split/Stats 2"), HideLabel, LabelWidth(10)]
        public int Cap2;
        [VerticalGroup("Split/Stats 2"), HideLabel, LabelWidth(10)]
        public int Cap3;

        [HorizontalGroup("Split", 0)]

        //[TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 2"), LabelWidth(50), HideInInspector]
        public float Armour;
        [VerticalGroup("Split/Stats 2"), LabelWidth(50), HideInInspector]
        public float Damage;
        [VerticalGroup("Split/Stats 2"), LabelWidth(50), HideInInspector]
        public float Exp;

        [HorizontalGroup("Split", 0)]

        //[TableColumnWidth(10)]
        [VerticalGroup("Split/Stats 4"), HideLabel, HideInInspector]
        public int Cap4;
        [VerticalGroup("Split/Stats 4"), HideLabel, HideInInspector]
        public int Cap5;
        [VerticalGroup("Split/Stats 4"), HideLabel, HideInInspector]
        public int Cap6;
    }
    
    public class EnemiesStatsRange : ISpawnTables {
        public EnemiesStatsRange() { }
    
        [HorizontalGroup("Split", 250)]
    
        [TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 1"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Amount;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Health;
        [VerticalGroup("Split/Stats 1"), LabelWidth(50), MinMaxSlider(-10, 10, true)]
        public Vector2 Points;

        [HorizontalGroup("Split", 20)]

        [TableColumnWidth(10)]
        [VerticalGroup("Split/Stats 2"), HideLabel, LabelWidth(10)]
        public int Cap1;                          
        [VerticalGroup("Split/Stats 2"), HideLabel, LabelWidth(10)]
        public int Cap2;                          
        [VerticalGroup("Split/Stats 2"), HideLabel, LabelWidth(10)]
        public int Cap3;

        [HorizontalGroup("Split", 0)]

        //[TableColumnWidth(90)]
        [VerticalGroup("Split/Stats 3"), LabelWidth(50), MinMaxSlider(-10, 10, true), HideInInspector]
        public Vector2 Armour;
        [VerticalGroup("Split/Stats 3"), LabelWidth(50), MinMaxSlider(-10, 10, true), HideInInspector]
        public Vector2 Damage;
        [VerticalGroup("Split/Stats 3"), LabelWidth(50), MinMaxSlider(-10, 10, true), HideInInspector]
        public Vector2 Exp;

        [HorizontalGroup("Split", 0)]

        //[TableColumnWidth(10)]
        [VerticalGroup("Split/Stats 4"), HideLabel, HideInInspector]
        public int Cap4;
        [VerticalGroup("Split/Stats 4"), HideLabel, HideInInspector]
        public int Cap5;
        [VerticalGroup("Split/Stats 4"), HideLabel, HideInInspector]
        public int Cap6;
    }
}