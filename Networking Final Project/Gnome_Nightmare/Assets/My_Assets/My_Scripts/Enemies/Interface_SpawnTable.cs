using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
namespace EnemySpawners {
    public class Interface_SpawnTable : MonoBehaviour {
        public static Interface_SpawnTable instance;
        void Awake() {
            instance = this;
            //Find all Spawner_Managers that are on the field
            spawnerManager = FindObjectsOfType(typeof(Spawner_Manager)) as Spawner_Manager[];
            //Find all Spawner_Settings that are on the field
            spawnerSettings = FindObjectsOfType(typeof(Spawner_Settings)) as Spawner_Settings[];
            //Find all Spawner_Hubs that are on the field
            spawnerHub = FindObjectsOfType(typeof(Spawner_Hub)) as Spawner_Hub[];
        }
    
        public bool ToggleAll = false; //Toggle the state of evey spawner on the field
        public float TimeBetweenRounds = 10.0f; //Time Between Rounds
        public int CurrentLevel = 0; 
        public int OldLevel = -1;
        public bool EverythingDead = false; //Is everything dead?
        private GameObject EnemyInfoUI;
        public Spawner_Manager[] spawnerManager; //An array of all Spawner_Managers
        public Spawner_Settings[] spawnerSettings; //An array of all Spawner_Settings
        public Spawner_Hub[] spawnerHub; //An array of all Spawner_Hubs

        // Use this for initialization
        private void Start () {
            DeActivateAllSpawners();
            Invoke("DelayedStart", 0.1f);
        }
        //Used so that everything gets a chance to load before trying to accsess it
        private void DelayedStart() {
            CurrentLevel = OldLevel = 0;
            //Finds and updates the UI
            if (GameObject.Find("World").transform.Find("Screen_Menu").transform.Find("Enemy Info")) {
                EnemyInfoUI = GameObject.Find("World").transform.Find("Screen_Menu").transform.Find("Enemy Info").gameObject;
                //EnemyInfoUI.GetComponent<Text>().text = ("[" + CurrentLevel + "-Wave] [" + CheckAliveEnemyCount() + "/" + CheckTotalEnemyCount() + "-Enemies]");
                EnemyInfoUI.GetComponent<Text>().text = ("[" + CurrentLevel + "-Wave]");
            }
        }
    
        // Update is called once per frame
        private void Update () {
            if (ToggleAll) { return; } //If taggled off, don't do anything
            UpdateUI(); //updated UI
            if (CheckAliveEnemyCount() == 0) { EverythingDead = true; } //Check if eveything is dead
            else { EverythingDead = false; }
            //At the end of a round, spawn a new round
            if (OldLevel == CurrentLevel && EverythingDead) {
                //Activate all spawners
                //ActivateAllSpawnersInCurrentRound();
            }
        }
    
        //Updates the UI and re-activates spawners at the end of the round
        public void UpdateUI() {
            //Update UI
            if (EnemyInfoUI != null) { EnemyInfoUI.GetComponent<Text>().text = ("[" + CurrentLevel + "-Wave]"); }
        }
    
        //Checks the amount of enemies currently alive
        private int CheckAliveEnemyCount() {
            //Finds where the enemies are spawned to and returns the amount of childern (ie. number of enemies)
            if (GameObject.Find("World").transform.Find("Enemies")) {
                return GameObject.Find("World").transform.Find("Enemies").childCount;
            }
            else { return 0; }
        }
    
        //Checks the total amount of enemies that will spawn per round
        private int CheckTotalEnemyCount() {
            int EnemyCount = 0;
            //Goes through each spawner and counts The amount of enemies it will spawn
            for (int i = 0; i < spawnerManager.Length; i++) {
                for (int j = 0; j < spawnerManager[i].enemySpawnTable.spawners.Count; j++) {
                    //If spawner is not active or it does not have the component than return
                    if (ToggleAll || spawnerManager[i].enemySpawnTable.spawners[j].Toggle || spawnerManager[i].enemySpawnTable.spawners[j].SpawnerPosition == null) { }
                    //enemySpawnTable.spawners[i].LastAvtiveRound will be set as the current round... (only for this)
                    else if ((CurrentLevel >= spawnerManager[i].enemySpawnTable.spawners[j].StartAt && CurrentLevel == spawnerManager[i].enemySpawnTable.spawners[j].LastAvtiveRound) || (CurrentLevel == spawnerManager[i].enemySpawnTable.spawners[j].StartAt)) {
                        //counts amount of enemies to spawn multiplyed by number of waves in spawner
                        EnemyCount += (spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.TotalEnemyCount() * spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.NumberOfWaves);
                    }
                    //spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.DefeatedSpawner = true;
                }
            }
            for (int i = 0; i < spawnerSettings.Length; i++) {
                //spawnerSettings[i].spawner.spawnerDetails.DefeatedSpawner = true;
                //If spawner is not active or it does not have the component than return
                if (ToggleAll || spawnerSettings[i].spawner.Toggle) { }
                //enemySpawnTable.spawners[i].LastAvtiveRound will be set as the current round... (only for this)
                else if ((CurrentLevel >= spawnerSettings[i].spawner.StartAt && CurrentLevel == spawnerSettings[i].spawner.LastAvtiveRound) || (CurrentLevel == spawnerSettings[i].spawner.StartAt)) {
                    //counts amount of enemies to spawn multiplyed by number of waves in spawner
                    EnemyCount += (spawnerSettings[i].spawner.spawnerDetails.TotalEnemyCount() * spawnerSettings[i].spawner.spawnerDetails.NumberOfWaves);
                }
            }
            for (int i = 0; i < spawnerHub.Length; i++)  {
                for (int j = 0; j < spawnerHub[i].EnemiesToSpawn.Count; j++) {

                    //Debug.Log("[CL " + CurrentLevel + "] [AE " + (spawnerHub[i].EnemiesToSpawn[j].LastAvtiveRound ) + "] [SA " + spawnerHub[i].EnemiesToSpawn[j].StartAt + "] [EC " + spawnerHub[i].TotalEnemyCount() + "]");
                    //If spawner is not active or it does not have the component than return
                    if (ToggleAll) { }
                    //enemySpawnTable.spawners[i].LastAvtiveRound will be set as the current round... (only for this)
                    else if (CurrentLevel == (spawnerHub[i].EnemiesToSpawn[j].LastAvtiveRound) || (CurrentLevel == spawnerHub[i].EnemiesToSpawn[j].StartAt)) {
                        //counts amount of enemies to spawn multiplyed by number of waves in spawner
                        EnemyCount += (int)(spawnerHub[i].EnemiesToSpawn[j].StartingAmount + spawnerHub[i].EnemiesToSpawn[j].IncreasedAmount);
                    }
                }
            }
    
            return EnemyCount;
        }
    
        //Checks if all spawners are DeActive
        public void CheckAllSpawners() {
            int countDeActive = 0;
            int totalSpawnerCount = 0;
            
            //Goes through each spawner and checks if they are de-active
            for (int i = 0; i < spawnerManager.Length; i++) {
                totalSpawnerCount += spawnerManager[i].enemySpawnTable.spawners.Count;
                for (int j = 0; j < spawnerManager[i].enemySpawnTable.spawners.Count; j++) {
                    if (spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.DefeatedSpawner) {
                        countDeActive += 1;
                    }
                }
            }

            //Goes through each spawner and checks if they are de-active
            totalSpawnerCount += spawnerSettings.Length;
            for (int i = 0; i < spawnerSettings.Length; i++) {
                if (spawnerSettings[i].spawner.spawnerDetails.DefeatedSpawner) {
                    countDeActive += 1;
                }
            }
            //Goes through each spawner and checks if they are de-active
            totalSpawnerCount += spawnerHub.Length;
            for (int i = 0; i < spawnerHub.Length; i++) {
                if (spawnerHub[i].isOff) {
                    countDeActive += 1;
                }
            }
    
            //If all spawners are de-active than Allow it to go to the next level (ie. if(OldLevel == CurrentLevel){ActivateAllSpawners();})
            if (countDeActive == totalSpawnerCount) { OldLevel = CurrentLevel; } //Debug.Log("All are DeActive");
            //If some spawners are still active
            else if (countDeActive < totalSpawnerCount) { }//Debug.Log((spawners.Count - countDeActive) + " are still Active");
            //um... Somehow there are more de-active spawners than there are spawners
            else { Debug.Log("Somehow there are " + (countDeActive - totalSpawnerCount) + " more DeActive spawners than there are spawners."); }
        }
    
        //Activates all spawners in current round
        public void ActivateAllSpawnersInCurrentRound() {
            CurrentLevel += 1;
			HellRoundUIColorChange.doCheck = true;
            if (Event_Manager.instance != null) { Event_Manager.instance.eventRoundProgress++; }

            //Goes through each spawner and sets the them to deactive
            for (int i = 0; i < spawnerManager.Length; i++) {
                for (int j = 0; j < spawnerManager[i].enemySpawnTable.spawners.Count; j++) {
                    //If spawner is not active or it does not have the component than return
                    if (spawnerManager[i].enemySpawnTable.spawners[j].Toggle) { }
                    else if ((CurrentLevel >= spawnerManager[i].enemySpawnTable.spawners[j].StartAt && CurrentLevel == (spawnerManager[i].enemySpawnTable.spawners[j].LastAvtiveRound + spawnerManager[i].enemySpawnTable.spawners[j].ActiveEvery)) || (CurrentLevel == spawnerManager[i].enemySpawnTable.spawners[j].StartAt)) {
                        spawnerManager[i].enemySpawnTable.spawners[j].LastAvtiveRound = CurrentLevel;
                        //If the spawner is deactive then active it
                        if (spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.DefeatedSpawner) {
                            spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.DefeatedSpawner = false;
                            spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.Round = TimeBetweenRounds;
                            spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.spawnCoolDownRemaining = TimeBetweenRounds;
                            spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.IncreaseStats();
                        }
                    }
                }
            }
            for (int i = 0; i < spawnerSettings.Length; i++) {
                //spawnerSettings[i].spawner.spawnerDetails.DefeatedSpawner = true;
                //If spawner is not active or it does not have the component than return
                if (spawnerSettings[i].spawner.Toggle) { }
                else if ((CurrentLevel >= spawnerSettings[i].spawner.StartAt && CurrentLevel == (spawnerSettings[i].spawner.LastAvtiveRound + spawnerSettings[i].spawner.ActiveEvery)) || (CurrentLevel == spawnerSettings[i].spawner.StartAt)) {
                    spawnerSettings[i].spawner.LastAvtiveRound = CurrentLevel;
                    //If the spawner is deactive then active it
                    if (spawnerSettings[i].spawner.spawnerDetails.DefeatedSpawner) {
                        spawnerSettings[i].spawner.spawnerDetails.DefeatedSpawner = false;
                        spawnerSettings[i].spawner.spawnerDetails.Round = TimeBetweenRounds;
                        spawnerSettings[i].spawner.spawnerDetails.spawnCoolDownRemaining = TimeBetweenRounds;
                        spawnerSettings[i].spawner.spawnerDetails.IncreaseStats();
                    }
                }
            }
            for (int i = 0; i < spawnerHub.Length; i++)  {
                spawnerHub[i].RoundInterval = TimeBetweenRounds;
                spawnerHub[i].CoolDown = TimeBetweenRounds;
                if (spawnerHub[i].Toggle) { return; }
                else { spawnerHub[i].isOff = false; }

                for (int j = 0; j < spawnerHub[i].EnemiesToSpawn.Count; j++) {
                    //If spawner is not active or it does not have the component than return
                    if (CurrentLevel == (spawnerHub[i].EnemiesToSpawn[j].LastAvtiveRound + spawnerHub[i].EnemiesToSpawn[j].ActiveEvery) || (CurrentLevel == spawnerHub[i].EnemiesToSpawn[j].StartAt)) {
                        spawnerHub[i].EnemiesToSpawn[j].LastAvtiveRound = CurrentLevel;
                        //If the spawner is deactive then active it
                        if (spawnerHub[i].EnemiesToSpawn[j].isOff) {
                            spawnerHub[i].EnemiesToSpawn[j].isOff = false;
                            spawnerHub[i].EnemiesToSpawn[j].IncreaseStats();
                        }
                    }
                }
            }
        }
    
        //Sets all the spawners to Deactive
        public void DeActivateAllSpawners() {
            //Goes through each spawner and sets the them to deactive
            for (int i = 0; i < spawnerManager.Length; i++) {
                for (int j = 0; j < spawnerManager[i].enemySpawnTable.spawners.Count; j++) {
                    spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.DefeatedSpawner = true;
                }
            }
            //Goes through each spawner and sets the them to deactive
            for (int i = 0; i < spawnerSettings.Length; i++) {
                spawnerSettings[i].spawner.spawnerDetails.DefeatedSpawner = true;
            }
            //Goes through each spawner and sets the them to deactive
            for (int i = 0; i < spawnerHub.Length; i++) {
                spawnerHub[i].isOff = true;
                for (int j = 0; j < spawnerHub[i].EnemiesToSpawn.Count; j++) {
                    spawnerHub[i].EnemiesToSpawn[j].isOff = true;
                }
            }
        }
    
        //Sets the amount of time between spawns
        public void SetAllIntervalBetweenSpawns(float amount) {
            //Goes through each spawner and sets the interval between spawns
            for (int i = 0; i < spawnerManager.Length; i++) {
                for (int j = 0; j < spawnerManager[i].enemySpawnTable.spawners.Count; j++) {
                    spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.Spawn = amount;
                }
            }
            //Goes through each spawner and sets the interval between spawns
            for (int i = 0; i < spawnerSettings.Length; i++) {
                spawnerSettings[i].spawner.spawnerDetails.Spawn = amount;
            }
            //Goes through each spawner and sets the interval between spawns
            for (int i = 0; i < spawnerHub.Length; i++) {
                spawnerHub[i].SpawnInterval = amount;
            }
        }
        //Sets the amount of time between rounds
        public void SetAllIntervalBetweenRounds(float amount) {
            //Goes through each spawner and sets the interval between rounds
            for (int i = 0; i < spawnerManager.Length; i++) {
                for (int j = 0; j < spawnerManager[i].enemySpawnTable.spawners.Count; j++) {
                    spawnerManager[i].enemySpawnTable.spawners[j].spawnersDetails.Round = amount;
                }
            }
            //Goes through each spawner and sets the interval between rounds
            for (int i = 0; i < spawnerSettings.Length; i++) {
                spawnerSettings[i].spawner.spawnerDetails.Round = amount;
            }
            //Goes through each spawner and sets the interval between rounds
            for (int i = 0; i < spawnerHub.Length; i++) {
                spawnerHub[i].RoundInterval = amount;
            }
        }
    
    }
}

