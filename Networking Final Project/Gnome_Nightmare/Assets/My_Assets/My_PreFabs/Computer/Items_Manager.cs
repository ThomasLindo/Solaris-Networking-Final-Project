using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems; //used for drag and drop

public class Items_Manager : SerializedMonoBehaviour  {
    public static Items_Manager instance;
    void Awake() { instance = this; }

    [HorizontalGroup("Menus"), LabelWidth(100)]
    public GameObject DisplayContent;
    [HorizontalGroup("Menus"), LabelWidth(50)]
    public GameObject Details;
    [HorizontalGroup("Menus"), LabelWidth(80)]
    public GameObject SpawnPoint;

    private int CurrentlySelectedTab = 0;
    private int CurrentlySelectedItem = 0;
    private Vector3 vec30 = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 vec31 = new Vector3(0.9f, 0.9f, 0.9f); //Vector3(1.0f, 1.0f, 1.0f);

    public OdinTables.OnlineTable OnlineTable = new OdinTables.OnlineTable();

    private void Start() { SetDisplayedContent(); SetInfo(0); }

    public int GetCurrentlySelectedTab() { return CurrentlySelectedTab; }
    public void SetCurrentlySelectedTab(int currentlySelectedTab) { CurrentlySelectedTab = currentlySelectedTab; SetDisplayedContent(); SetInfo(0); }
    public int GetCurrentlySelectedItem() { return CurrentlySelectedItem; }
    public void SetCurrentlySelectedItem(int currentlySelectedItem) { CurrentlySelectedItem = currentlySelectedItem; }


    private void SetDisplayedContent(){
        DestroyChildren();
        if (CurrentlySelectedTab == 0) {
            for (int i = 0; i < OnlineTable.Ammo.Count; i++) {
                GameObject Temp = Instantiate<GameObject>(OnlineTable.Ammo[i].Item);
                Temp.name = OnlineTable.Ammo[i].Item.name;
                Temp.transform.SetParent(DisplayContent.transform);
                Temp.GetComponent<RectTransform>().localPosition = vec30;
                Temp.GetComponent<RectTransform>().localScale = vec31;
            }
            Vector2 Temp2 = DisplayContent.GetComponent<RectTransform>().sizeDelta;
            Temp2.y = (210.0f + (34.0f * (Mathf.Clamp(OnlineTable.Ammo.Count - 3, 0, 100000))));
            DisplayContent.GetComponent<RectTransform>().sizeDelta = Temp2;
        }
        else if (CurrentlySelectedTab == 1) { 
            for (int i = 0; i < OnlineTable.Parts.Count; i++) {
                GameObject Temp = Instantiate<GameObject>(OnlineTable.Parts[i].Item);
                Temp.name = OnlineTable.Parts[i].Item.name;
                Temp.transform.SetParent(DisplayContent.transform);
                Temp.GetComponent<RectTransform>().localPosition = vec30;
                Temp.GetComponent<RectTransform>().localScale = vec31;
            }
            Vector2 Temp2 = DisplayContent.GetComponent<RectTransform>().sizeDelta;
            Temp2.y = (210.0f + (34.0f * (Mathf.Clamp(OnlineTable.Parts.Count - 3, 0, 100000))));
            DisplayContent.GetComponent<RectTransform>().sizeDelta = Temp2;
        }
        else if (CurrentlySelectedTab == 2) { 
            for (int i = 0; i < OnlineTable.Misc.Count; i++) {
                GameObject Temp = Instantiate<GameObject>(OnlineTable.Misc[i].Item);
                Temp.name = OnlineTable.Misc[i].Item.name;
                Temp.transform.SetParent(DisplayContent.transform);
                Temp.GetComponent<RectTransform>().localPosition = vec30;
                Temp.GetComponent<RectTransform>().localScale = vec31;
            }
            Vector2 Temp2 = DisplayContent.GetComponent<RectTransform>().sizeDelta;
            Temp2.y = (210.0f + (34.0f * (Mathf.Clamp(OnlineTable.Misc.Count - 3, 0, 100000))));
            DisplayContent.GetComponent<RectTransform>().sizeDelta = Temp2;
        }
    }

    private void DestroyChildren() {
        foreach (Transform child in DisplayContent.transform) { Destroy(child.gameObject); }
    }


    public void SetInfo(int childIndex) {
        if (GetCurrentlySelectedTab() == 0) {
            Details.transform.GetChild(0).GetComponent<Text>().text = OnlineTable.Ammo[childIndex].Item.name;
            Details.transform.GetChild(1).GetComponent<Text>().text = OnlineTable.Ammo[childIndex].Summary;
            Details.transform.GetChild(2).GetComponent<Text>().text = OnlineTable.Ammo[childIndex].Price.ToString();
        }
        else if (GetCurrentlySelectedTab() == 1) {
            Details.transform.GetChild(0).GetComponent<Text>().text = OnlineTable.Parts[childIndex].Item.name;
            Details.transform.GetChild(1).GetComponent<Text>().text = OnlineTable.Parts[childIndex].Summary;
            Details.transform.GetChild(2).GetComponent<Text>().text = OnlineTable.Parts[childIndex].Price.ToString();
        }
        else if (GetCurrentlySelectedTab() == 2) { 
            Details.transform.GetChild(0).GetComponent<Text>().text = OnlineTable.Misc[childIndex].Item.name;
            Details.transform.GetChild(1).GetComponent<Text>().text = OnlineTable.Misc[childIndex].Summary;
            Details.transform.GetChild(2).GetComponent<Text>().text = OnlineTable.Misc[childIndex].Price.ToString();
            
            if (OnlineTable.Misc[childIndex].Item.GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0) &&
                OnlineTable.Misc[childIndex].Item.GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).GetComponent<Gun_Behaviour>())
            {
                Gun_Behaviour weapon = OnlineTable.Misc[childIndex].Item.GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).GetComponent<Gun_Behaviour>();
                Melee_Behaviour melee = OnlineTable.Misc[childIndex].Item.GetComponent<Drag_Inventory>().ItemOnDrop.transform.GetChild(0).GetComponent<Melee_Behaviour>();
                ItemStats item = OnlineTable.Misc[childIndex].Item.GetComponent<ItemStats>();
                if (weapon.WeaponTypeHitScan || weapon.WeaponTypeProjectile) {
                    string Summary = OnlineTable.Misc[childIndex].Summary;
                    Summary += "\n" + weapon.TypeOfAmmo.ToString() + " Ammo";
                    Summary += "\n" + item.itemStats.Damage.baseValue.ToString() + " Damage";
                    Summary += "\n" + item.itemStats.Range.baseValue.ToString() + " Range";
                    Summary += "\n" + item.itemStats.FireRate.baseValue.ToString() + " Rate of Fire";
                    Details.transform.GetChild(1).GetComponent<Text>().text = Summary;
                }
                else if (melee) {
                    string Summary = OnlineTable.Misc[childIndex].Summary;
                    Summary += "\n" + item.itemStats.Damage.baseValue.ToString() + " Damage";
                    Summary += "\n" + item.itemStats.FireRate.baseValue.ToString() + " Rate of Fire";
                    Details.transform.GetChild(1).GetComponent<Text>().text = Summary;
                }
            }
        }
    }


    public void SpawnItem() {
        if (GetCurrentlySelectedTab() == 0 && PlayerManager.instance.GetComponent<PlayerStats>().CheckPoints(OnlineTable.Ammo[CurrentlySelectedItem].Price)) {
            PlayerManager.instance.GetComponent<PlayerStats>().UsePoints(OnlineTable.Ammo[CurrentlySelectedItem].Price);
            GameObject Item = (GameObject)Instantiate(OnlineTable.Ammo[CurrentlySelectedItem].Item.GetComponent<Drag_Inventory>().ItemOnDrop);
            Item.transform.SetParent(GameObject.FindWithTag("Items_Spawn_Here").transform);
            Item.name = OnlineTable.Ammo[CurrentlySelectedItem].Item.GetComponent<Drag_Inventory>().ItemOnDrop.name;
            Vector3 tempPos = SpawnPoint.transform.position;
            Item.transform.position = new Vector3(tempPos.x, tempPos.y + 0.50f, tempPos.z);
            if (OnlineTable.Ammo[CurrentlySelectedItem].Item.GetComponent<ItemStats>() && Item.transform.GetChild(0).GetComponent<Gun_Behaviour>())  {
                OdinTables.WeaponStatsTable FromStats = OnlineTable.Ammo[CurrentlySelectedItem].Item.GetComponent<ItemStats>().itemStats;
                OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
                ToStats.SetStats(FromStats, ToStats);
            }
            
            //Instantiate the Agent so that it will send to the other clients
            Agent tempAgent;
            if (Item.GetComponent<Agent>()) { tempAgent = Item.GetComponent<Agent>(); }
            else {
                Item.gameObject.AddComponent<Agent>();
                tempAgent = Item.GetComponent<Agent>();
            }
            tempAgent.AgentNumber = ID_Table.instance.ItemList[0];
            ID_Table.instance.ItemList.RemoveAt(0);
            tempAgent.RepeatEvery = 10.0f;
            tempAgent.SendInstantiate(Item.transform.position);

        }
        else if (GetCurrentlySelectedTab() == 1 && PlayerManager.instance.GetComponent<PlayerStats>().CheckPoints(OnlineTable.Parts[CurrentlySelectedItem].Price)) {
            PlayerManager.instance.GetComponent<PlayerStats>().UsePoints(OnlineTable.Parts[CurrentlySelectedItem].Price);
            GameObject Item = (GameObject)Instantiate(OnlineTable.Parts[CurrentlySelectedItem].Item.GetComponent<Drag_Inventory>().ItemOnDrop);
            Item.transform.SetParent(GameObject.FindWithTag("Items_Spawn_Here").transform);
            Item.name = OnlineTable.Parts[CurrentlySelectedItem].Item.GetComponent<Drag_Inventory>().ItemOnDrop.name;
            Vector3 tempPos = SpawnPoint.transform.position;
            Item.transform.position = new Vector3(tempPos.x, tempPos.y + 0.50f, tempPos.z);
            if (OnlineTable.Parts[CurrentlySelectedItem].Item.GetComponent<ItemStats>() && Item.transform.GetChild(0).GetComponent<Gun_Behaviour>())  {
                OdinTables.WeaponStatsTable FromStats = OnlineTable.Parts[CurrentlySelectedItem].Item.GetComponent<ItemStats>().itemStats;
                OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
                ToStats.SetStats(FromStats, ToStats);
            }
            
            //Instantiate the Agent so that it will send to the other clients
            Agent tempAgent;
            if (Item.GetComponent<Agent>()) { tempAgent = Item.GetComponent<Agent>(); }
            else {
                Item.gameObject.AddComponent<Agent>();
                tempAgent = Item.GetComponent<Agent>();
            }
            tempAgent.AgentNumber = ID_Table.instance.ItemList[0];
            ID_Table.instance.ItemList.RemoveAt(0);
            tempAgent.RepeatEvery = 10.0f;
            tempAgent.SendInstantiate(Item.transform.position);

        }
        else if (GetCurrentlySelectedTab() == 2 && PlayerManager.instance.GetComponent<PlayerStats>().CheckPoints(OnlineTable.Misc[CurrentlySelectedItem].Price)) {
            PlayerManager.instance.GetComponent<PlayerStats>().UsePoints(OnlineTable.Misc[CurrentlySelectedItem].Price);
            GameObject Item = (GameObject)Instantiate(OnlineTable.Misc[CurrentlySelectedItem].Item.GetComponent<Drag_Inventory>().ItemOnDrop);
            Item.transform.SetParent(GameObject.FindWithTag("Items_Spawn_Here").transform);
            Item.name = OnlineTable.Misc[CurrentlySelectedItem].Item.GetComponent<Drag_Inventory>().ItemOnDrop.name;
            Vector3 tempPos = SpawnPoint.transform.position;
            Item.transform.position = new Vector3(tempPos.x, tempPos.y + 0.50f, tempPos.z);

            if (OnlineTable.Misc[CurrentlySelectedItem].Item.GetComponent<ItemStats>() && Item.transform.GetChild(0).GetComponent<Gun_Behaviour>())  {
                OdinTables.WeaponStatsTable FromStats = OnlineTable.Misc[CurrentlySelectedItem].Item.GetComponent<ItemStats>().itemStats;
                OdinTables.WeaponStatsTable ToStats = Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
                ToStats.SetStats(FromStats, ToStats);
            }

            //Instantiate the Agent so that it will send to the other clients
            Agent tempAgent;
            if (Item.GetComponent<Agent>()) { tempAgent = Item.GetComponent<Agent>(); }
            else {
                Item.gameObject.AddComponent<Agent>();
                tempAgent = Item.GetComponent<Agent>();
            }
            tempAgent.AgentNumber = ID_Table.instance.ItemList[0];
            ID_Table.instance.ItemList.RemoveAt(0);
            tempAgent.RepeatEvery = 10.0f;
            tempAgent.SendInstantiate(Item.transform.position);


        }
    }
}

