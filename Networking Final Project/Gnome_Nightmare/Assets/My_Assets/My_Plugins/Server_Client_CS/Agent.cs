using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Agent : SerializedMonoBehaviour {

    //private Server_Manager server;
    //private Client_Manager client;
    private void Awake() {
        //if (Client_Manager.instance) { client = Client_Manager.instance; }
        //else if (Server_Manager.instance) { server = Server_Manager.instance; }
    }

    [BoxGroup("Info 1", false)]
    [HorizontalGroup("Info 1/1", 0.5f), LabelWidth(50)]
    public float StartIn = 1.0f;
    [HorizontalGroup("Info 1/1", 0.5f), LabelWidth(90)]
    public float RepeatEvery = 1.0f;
    [BoxGroup("Info 2", false)]
    [HorizontalGroup("Info 2/2", 0.5f), LabelWidth(90)]
    public int AgentNumber = -1;
    [HorizontalGroup("Info 2/2", 0.5f), LabelWidth(90)]
    public int PrefabNumber = -1;

    [BoxGroup("Stuff To Send", false)]
    [HorizontalGroup("Stuff To Send/1"), LabelWidth(50)]
    public bool Position = false;
    [HorizontalGroup("Stuff To Send/1"), LabelWidth(50)]
    public bool Rotation = false;
    [HorizontalGroup("Stuff To Send/1"), LabelWidth(40)]
    public bool Health = false;
    public bool CantDie = false;

    private EnemyStats enemyHealth;
    private PlayerStats playerHealth;
    private Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 rot = new Vector3(0.0f, 0.0f, 0.0f);
    private float health = 0.0f;

    [HideInInspector]
    public Vector3 TargetPos = new Vector3(0.0f, 0.0f, 0.0f);
    [HideInInspector]
    public Vector3 TargetRot = new Vector3(0.0f, 0.0f, 0.0f);
    public float LerpSpeed = 0.2f;

    private bool IsLonely = false;

    private void Start() {
        if (this.GetComponent<EnemyStats>()) { enemyHealth = this.GetComponent<EnemyStats>(); }
        if (this.GetComponent<PlayerStats>()) { playerHealth = this.GetComponent<PlayerStats>(); }
        Invoke("LateStart", 0.50f);
    }
    private void LateStart() {
        // Repeat X, in Y seconds, every Z seconds.
        if (Client_Manager.instance) { InvokeRepeating("ClientRepeatThis", StartIn, RepeatEvery); }
        else if (Server_Manager.instance) { InvokeRepeating("ServerRepeatThis", StartIn, RepeatEvery); }
        else { IsLonely = true; }
    }
    private void Update() {
        //If Agent is not at Target and not at (0,0,0)
        if (TargetPos != this.gameObject.transform.position && TargetPos != Vector3.zero) {
            //If the Target position is a long distance away just teloport it to the Target position
            if (Vector3.Distance(TargetPos, this.gameObject.transform.position) > 10.0f) { this.gameObject.transform.position = TargetPos; }
            else { this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, TargetPos, LerpSpeed); }
        }
        //If Agent is not at Target and not at (0,0,0)
        if (TargetRot != this.gameObject.transform.rotation.eulerAngles && TargetRot != Vector3.zero) {
            //If the Target position is a long distance away just teloport it to the Target position
            this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, Quaternion.Euler(TargetRot), LerpSpeed);
        }
    }





    private string GetDataToSend(string temp) {
        if (Position) {
            if (pos != this.gameObject.transform.position) {
                pos = this.gameObject.transform.position;
                temp += ("&P(" + System.Math.Round(pos.x, 2).ToString() + "," + System.Math.Round(pos.y, 2).ToString() + "," + System.Math.Round(pos.z, 2).ToString() + ")");
            }
        }
        if (Rotation) {
            if (rot != this.gameObject.transform.rotation.eulerAngles) {
                rot = this.gameObject.transform.rotation.eulerAngles;
                temp += ("&R(" + System.Math.Round(rot.x, 2).ToString() + "," + System.Math.Round(rot.y, 2).ToString() + "," + System.Math.Round(rot.z, 2).ToString() + ")");
            }
        }
        if (Health) {
            if (enemyHealth && health != enemyHealth.CurrentHealth) { health = enemyHealth.CurrentHealth; temp += ("&HP" + enemyHealth.CurrentHealth.ToString() + "|"); }
            if (playerHealth && health != playerHealth.CurrentHealth) { health = playerHealth.CurrentHealth; temp += ("&HP" + playerHealth.CurrentHealth.ToString() + "|"); }
        }
        return temp;
    }

    public void SendInstantiate() {
        if (IsLonely) { return; }
        if (PrefabNumber == -1 || AgentNumber == -1) { Debug.Log("Could not Instantiate"); return; }

        string temp = ("@" + PrefabNumber.ToString() + "|" + "#" + AgentNumber.ToString() + "|");
        temp = GetDataToSend(temp);

        if (Client_Manager.instance) { Client_Manager.instance.SendData(temp); Debug.Log("Client Instantiate"); }
        else if (Server_Manager.instance) { Server_Manager.instance.SendData(temp); Debug.Log("Server Instantiate"); }
        else { Invoke("SendInstantiate", 0.50f); }
    }
    public void SendInstantiate(Vector3 InitialPos) {
        if (IsLonely) { return; }
        if (PrefabNumber == -1 || AgentNumber == -1) { Debug.Log("Could not Instantiate"); return; }

        string temp = ("@" + PrefabNumber.ToString() + "|" + "#" + AgentNumber.ToString() + "|" + "&P(" + InitialPos.x.ToString() + "," + InitialPos.y.ToString() + "," + InitialPos.z.ToString() + ")");
        temp = GetDataToSend(temp);

        if (Client_Manager.instance) { Client_Manager.instance.SendData(temp); Debug.Log("Client Instantiate"); }
        else if (Server_Manager.instance) { Server_Manager.instance.SendData(temp); Debug.Log("Server Instantiate"); }
        else { Invoke("SendInstantiate", 0.50f); }
    }

    private void ClientRepeatThis() {
        if (AgentNumber == -1) { return; }

        string temp = ("#" + AgentNumber.ToString() + "|");
        temp = GetDataToSend(temp);

        if (temp != ("#" + AgentNumber.ToString() + "|")) { /*Debug.Log("Client: " + temp);*/ Client_Manager.instance.SendData(temp); }
    }
    private void ServerRepeatThis() {
        if (AgentNumber == -1) { return; }

        string temp = ("#" + AgentNumber.ToString() + "|");
        temp = GetDataToSend(temp);

        if (temp != ("#" + AgentNumber.ToString() + "|")) { /*Debug.Log("Server: " + temp);*/ Server_Manager.instance.SendData(temp); }
    }

    public void SendDestroy() {
        if (IsLonely) { return; }
        if (AgentNumber == -1) { return; }
        
        //If the ID is for an Item Add it back to the ItemList
        if (AgentNumber >= 300 && AgentNumber <= 500) { ID_Table.instance.ItemList.Add(AgentNumber); }
        //[~] Destroy [#|] Agent ID
        string temp = ("~#" + AgentNumber.ToString() + "|");

        //Debug.Log(temp + " died");

        if (Client_Manager.instance) { Client_Manager.instance.SendData(temp); Debug.Log("Client Destroy"); }
        else if (Server_Manager.instance) { Server_Manager.instance.SendData(temp); Debug.Log("Server Destroy"); }
        else { Invoke("SendDestroy", 0.50f); }
    }

    private void OnDestroy() { SendDestroy(); }
}
