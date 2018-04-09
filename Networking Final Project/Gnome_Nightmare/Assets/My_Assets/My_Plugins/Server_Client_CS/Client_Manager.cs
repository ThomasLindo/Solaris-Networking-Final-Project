

using UnityEngine;
using UnityEngine.UI;
using ServerDll;
using Sirenix.OdinInspector;

public class Client_Manager : SerializedMonoBehaviour {

    public int PortNumber = 8080;
    public static Client_Manager instance;
    //private ID_Table IDTable;
    //private Tutorial_Manager TM;
    //private Event_Manager ET;
    public int PlayerNumber = -1;
    private bool InGame = false;
    private ServerDll.Client client = new ServerDll.Client();
    public bool ClientOn = false;
    public string ClientMessage = "~@4|#4|&P(1.2,3.4,5.6)&R(9.8,7.6,5.4)&HP26|";

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start() {
        client.SetPort(PortNumber);
        //IDTable = ID_Table.instance;
        //ET = Event_Manager.instance;
    }

    [Button]
    public void ConnectToServer() {
        if (client.TCP_ConnectToServer()) { ClientOn = true; }
    }
    [Button]
    public void Send() {
        client.TCP_SendData(ClientMessage);
    }
    public void SendData(string data) {
        client.TCP_SendData(data);
    }


    // Update is called once per frame
    private void Update () {
        //if (IDTable == null) { IDTable = ID_Table.instance; }
        //if (TM == null) { TM = GameObject.FindObjectOfType(typeof(Tutorial_Manager)) as Tutorial_Manager; }

        if (!InGame) {
            string TCP_Data = client.TCP_GetData();
            if (!string.IsNullOrEmpty(TCP_Data)) {
                if (TCP_Data.Contains("&StartGame|")) {
                    var temp = GameObject.FindObjectOfType(typeof(LoadScenes)) as LoadScenes;
                    temp.LoadSceneFromString("Gnome_Nightmare");
                    InGame = true;
                }
            }
            return;
        }

        if (ClientOn) {
            string TCP_Data = client.TCP_GetData();
            if (!string.IsNullOrEmpty(TCP_Data)) {
                ProcessData(TCP_Data);
            }
        }
	}



    
    private void ProcessData(string data) {
        bool destroy = false;
        int instantiate = -1;
        int ID = -1;
        Vector3 pos = new Vector3();
        Vector3 rot = new Vector3();
        Vector3 evt = new Vector3();
        float hp = -1;


        //Destroy an object
        if (data.Contains("~")) {
            destroy = true;
            data = data.Substring(1);
        }
        //Instantiate an object
        if (data.Contains("@")) {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            instantiate = int.Parse(t);
            data = data.Substring(t.Length+2);
        }
        //ID of an object
        if (data.Contains("#"))  {
            string t = data.Split('|')[0];
            t = t.Substring(1);
            ID = int.Parse(t);
            data = data.Substring(t.Length+2);
        }
        //Position of an object
        if (data.Contains("&P"))  {
            string t = data.Split('(')[1]; t = t.Split(')')[0];
            float x = float.Parse(t.Split(',')[0]);
            float y = float.Parse(t.Split(',')[1]);
            float z = float.Parse(t.Split(',')[2]);
            pos = new Vector3(x,y,z);
            data = data.Substring(t.Length+4);
        }
        //Rotation of an object
        if (data.Contains("&R"))  {
            string t = data.Split('(')[1]; t = t.Split(')')[0];
            float x = float.Parse(t.Split(',')[0]);
            float y = float.Parse(t.Split(',')[1]);
            float z = float.Parse(t.Split(',')[2]);
            rot = new Vector3(x,y,z);
            data = data.Substring(t.Length+4);
        }
        //Health of an object
        if (data.Contains("&HP")) {
            string t = data.Split('|')[0];
            t = t.Substring(3);
            hp = float.Parse(t);
            data = data.Substring(t.Length + 4);
        }
        //Tutorial Stage
        if (data.Contains("&TS")) {
            string t = data.Split('|')[0];
            t = t.Substring(3);
            Tutorial_Manager.instance.Stage = int.Parse(t);
            data = data.Substring(t.Length + 4);
        }
        //Current Event
        if (data.Contains("!")) {
            string t = data.Split('(')[1]; t = t.Split(')')[0];
            float x = float.Parse(t.Split(',')[0]);
            float y = float.Parse(t.Split(',')[1]);
            float z = float.Parse(t.Split(',')[2]);
            evt = new Vector3(x, y, z);
            data = data.Substring(t.Length + 3);
            Event_Manager.instance.SetEvent(evt);
        }
        //New ID
        if (data.Contains("&NID"))  {
            string t = data.Split('|')[0];
            t = t.Substring(4);
            int NID = int.Parse(t);
            data = data.Substring(t.Length+5);
            //Find Object with the same [ID] and change it.
            Agent[] agents = (Agent[]) GameObject.FindObjectsOfType(typeof(Agent));
            foreach (var agent in agents) {
                if (agent.AgentNumber == ID) { agent.AgentNumber = NID; break; }
            }
            ID = NID;
        }

        //Need ID to handle Events
        if (ID == -1) { return; }

        if (destroy) {
            Debug.Log("Destroy: " + ID);
            //Find Object with the same [ID] and Destroy it.
            Agent[] agents = (Agent[]) GameObject.FindObjectsOfType(typeof(Agent));
            foreach (var agent in agents) {
                if (agent.AgentNumber == ID) {
                    if (agent.CantDie) { return; }
                    Debug.Log("Destroyed: " + agent.gameObject.name + " | ID: " + ID);
                    //If the ID is for an Item Add it back to the ItemList
                    if (ID >= 300 && ID <= 500) { ID_Table.instance.ItemList.Add(ID); }
                    else if (ID >= 100 && ID <= 300) { agent.GetComponent<EnemyStats>().OnDeath(); }
                    //Destroy the object
                    else { Destroy(agent.gameObject); }
                }
            }
        }
        else if (instantiate != -1) {
            Debug.Log("Was in Table");
            //Instantiate and Object with [ID] from the ID_Table
            Debug.Log("Instantiate: " + instantiate + " | ID: " + ID);
            GameObject t;
            if (ID_Table.instance.IDTable.TryGetValue(instantiate, out t)) {
                GameObject temp = Instantiate(t, pos, Quaternion.Euler(rot));
                temp.name = t.name;
                //Set the Agents ID
                if (temp.GetComponent<Agent>()) { temp.GetComponent<Agent>().AgentNumber = ID; }
                //Create Agent component
                else {
                    temp.AddComponent<Agent>();
                    temp.GetComponent<Agent>().AgentNumber = ID;
                }
                //If the ID is for an Item Remove it from the ItemList
                if (ID >= 300 && ID <= 500) { ID_Table.instance.ItemList.Remove(ID); }
                //Set health
                if (hp != -1) {
                    if (temp.GetComponent<EnemyStats>())  { temp.GetComponent<EnemyStats>().MaxHealth = hp;  temp.GetComponent<EnemyStats>().CurrentHealth = hp; }
                    if (temp.GetComponent<PlayerStats>()) { temp.GetComponent<PlayerStats>().MaxHealth = hp; temp.GetComponent<PlayerStats>().CurrentHealth = hp; }
                }
                //If Taged as an Item, parent the Item to the Items tab.
                if (temp.tag == "Items") { temp.transform.SetParent(GameObject.FindGameObjectWithTag("Items_Spawn_Here").transform); }
                //If Taged as an Enemy, parent the Item to the Enemies tab.
                else if (temp.tag == "Enemy") { temp.transform.SetParent(GameObject.FindGameObjectWithTag("Enemies_Spawn_Here").transform); }
                //
            }
        }
        else {
            //Find Object with the same [ID] and change it.
            Agent[] agents = (Agent[]) GameObject.FindObjectsOfType(typeof(Agent));
            foreach (var agent in agents) {
                if (agent.AgentNumber == ID) {
                    //Make this Lerp
                    if (pos != Vector3.zero) { agent.TargetPos = pos; }
                    if (rot != Vector3.zero) { agent.TargetRot = rot; }
                    //if (pos != Vector3.zero) { agent.gameObject.transform.position = pos; }
                    //if (rot != Vector3.zero) { agent.gameObject.transform.rotation = Quaternion.Euler(rot); }
                    if (hp >= 0) {
                        if (agent.GetComponentInParent<EnemyStats>()) { agent.gameObject.GetComponent<EnemyStats>().CurrentHealth = hp; }
                        if (agent.GetComponentInParent<PlayerStats>()) { agent.gameObject.GetComponent<PlayerStats>().CurrentHealth = hp; }
                    }
                }
            }
        }
    }

    public void Quit() {
        client.TCP_CloseSocket();
        Destroy(this);
    }

    public void SetPlayerNumber(int num) {
        PlayerNumber = num;
    }
    public void SetPlayerNumber(string num) {
        int x = -1;
        int.TryParse(num, out x);
        PlayerNumber = x;
    }
    public void SetPlayerNumber(Text num) {
        int x = -1;
        int.TryParse(num.text, out x);
        PlayerNumber = x;
    }
    
}
