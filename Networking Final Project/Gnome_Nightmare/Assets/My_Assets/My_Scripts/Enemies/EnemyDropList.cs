using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random_Utils;


public class EnemyDropList : SerializedMonoBehaviour {

    public List<OdinTables.EnemyDropTable> Enemies = new List<OdinTables.EnemyDropTable>();

    public List<int> SpawnRandormDrop(int FromEnemy) {
        List<int> ItemsToSpawn = new List<int>();
        for (int i = 0; i < Enemies[FromEnemy].Items.Count; i++) {
            float Range = Enemies[FromEnemy].Items[i].DropChance;

            //If there is a 100 precent chance to drop it will drop
            if (Range == 100.0f) { ItemsToSpawn.Add(i); }
            //If there is a 0 precent chance to drop it will return
            else if (Range == 0.0f) { return null; }
            //If the chance is between 0 and 100 precent
            else {
                //First random number between 0 and 100
                float RandNum1 = RandomUtils.RandomFloat(0.0f, 100.0f);
                //make a range based on the First random number 
                Vector2 randRange = new Vector2(RandNum1-(Range*0.5f), RandNum1+(Range*0.5f));
                //Second random number between 0 and 100
                float RandNum2 = RandomUtils.RandomFloat(0.0f, 100.0f);

                //If the Second random number is between the range of the First random number add the number
                if ((RandNum2 > randRange.x && RandNum2 < randRange.y) || (RandNum2-100.0f > randRange.x && RandNum2-100.0f < randRange.y) || (RandNum2+100.0f > randRange.x && RandNum2+100.0f < randRange.y)) {
                    ItemsToSpawn.Add(i);
                    //Debug.Log("[" + randRange.x + "/" + randRange.y + "] [" + RandNum2 + "]");
                }
            }
        }
        return ItemsToSpawn;
    }
    public void SpawnItem(int FromEnemy, List<int> SpawnItem, Transform AtPosition) {
        //Go through all the Items that can spawn forn the spawn list
        for (int j = 0; j < SpawnItem.Count; j++) {
            //Go through all the Items that the Enemy can spawn
            for (int i = 0; i < Enemies[FromEnemy].Items.Count; i++) {

                //If the spawn list item is the same as the one the Enemy can spawn, than spawn it
                if (i == SpawnItem[j]) {
                    //for (int k = 0; k < Enemies[FromEnemy].Items[i].Amount; k++) {
                    //    GameObject TempItem = Instantiate<GameObject>(Enemies[FromEnemy].Items[i].Item);
                    //    TempItem.name = Enemies[FromEnemy].Items[i].Item.name;
                    //    TempItem.transform.SetParent(GameObject.Find("World").transform.Find("Items").transform);
                    //    TempItem.transform.localPosition = AtPosition.localPosition;
                    //}

                    //Spawn the Item
                    for (int k = 0; k < Enemies[FromEnemy].Items[i].Amount; k++) {
                        GameObject TempItem = (GameObject)Instantiate(Enemies[FromEnemy].Items[i].Item);
                        GameObject Item = (GameObject)Instantiate(TempItem.GetComponent<Drag_Inventory>().ItemOnDrop);

                        Item.name = TempItem.GetComponent<Drag_Inventory>().ItemOnDrop.name;
                        Item.transform.SetParent(GameObject.Find("World").transform.Find("Items").transform);
                        Item.transform.localPosition = AtPosition.localPosition + new Vector3(0.0f, 0.5f, 0.0f);


                        //Stuff happend and I dont need all of this, but keeping it any-way just in case.
                        if (TempItem.GetComponent<ItemStats>() && Item.transform.GetChild(0).GetComponent<Gun_Behaviour>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<ItemStats>().itemStats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats = ToStats;
                        }
                        else if (TempItem.GetComponent<ItemStats>() && Item.transform.GetChild(0).GetComponent<ItemStats>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<ItemStats>().itemStats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats = ToStats;
                        }
                        else if (TempItem.GetComponent<Gun_Behaviour>() && Item.transform.GetChild(0).GetComponent<ItemStats>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<Gun_Behaviour>().Stats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<ItemStats>().itemStats = ToStats;
                        }
                        else if (TempItem.GetComponent<Gun_Behaviour>() && Item.transform.GetChild(0).GetComponent<Gun_Behaviour>()) {
                            OdinTables.WeaponStatsTable FromStats = TempItem.GetComponent<Gun_Behaviour>().Stats;
                            OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
                            ToStats.SetStats(FromStats, ToStats);
                            Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats = ToStats;
                        }
                        Destroy(TempItem);

                        //Instantiate the Agent so that it will send to the other clients
                        Agent tempAgent;
                        if (Item.GetComponent<Agent>()) { tempAgent = Item.GetComponent<Agent>(); }
                        else {
                            Item.AddComponent<Agent>();
                            tempAgent = Item.GetComponent<Agent>();
                        }
                        tempAgent.AgentNumber = ID_Table.instance.ItemList[0];
                        ID_Table.instance.ItemList.RemoveAt(0);
                        tempAgent.RepeatEvery = 10.0f;
                        tempAgent.SendInstantiate(Item.transform.position);

                    }


                }
            }
        }
    }
}
