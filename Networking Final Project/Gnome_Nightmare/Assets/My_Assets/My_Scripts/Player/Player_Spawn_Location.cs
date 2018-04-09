using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random_Utils;

public class Player_Spawn_Location : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;
    private bool serverStarted;

    // Use this for initialization
    void Start () {
        Invoke("LateStart", 1.0f);
    }
    private void LateStart() {
        //Spawn the player
        GameObject player = (GameObject)Instantiate(PlayerPrefab, this.transform.position, this.transform.rotation);
        player.name = "Player";
        if (player.GetComponent<Agent>()) {
            if (Server_Manager.instance) {
                Debug.Log(Server_Manager.instance.PlayerNumber);
                player.GetComponent<Agent>().AgentNumber = Server_Manager.instance.PlayerNumber;
                //player.transform.GetChild(0).GetComponentInChildren<Agent>().AgentNumber = Server_Manager.instance.PlayerNumber + 1;
            }
            else if (Client_Manager.instance) {
                Debug.Log(Client_Manager.instance.PlayerNumber);
                player.GetComponent<Agent>().AgentNumber = Client_Manager.instance.PlayerNumber;
                //player.transform.GetChild(0).GetComponentInChildren<Agent>().AgentNumber = Client_Manager.instance.PlayerNumber - 1;
            }
            //Just in case //Though this will still case problems
            if (player.GetComponent<Agent>().AgentNumber == -1) {
                player.GetComponent<Agent>().AgentNumber = RandomUtils.RandomInt(2, 99);
                //player.transform.GetChild(0).GetComponentInChildren<Agent>().AgentNumber = player.GetComponent<Agent>().AgentNumber + 1;
            }

            player.GetComponent<Agent>().SendInstantiate();
        }
        //Spawn the Camera
        GameObject camera = (GameObject)Instantiate(CameraPrefab, this.transform.position, this.transform.rotation);
        camera.name = "Main Camera";
        camera.GetComponent<CameraFollow>().FollowThis = player.transform.Find("HitBox").Find("Top").Find("Head").gameObject;
        camera.GetComponent<CameraFollow>().endPoint = player.transform.Find("Endpoint").transform;
    }

    private void Update() {
        if (Client_Manager.instance) { Destroy(this, 2.0f); return; }
        //if (serverStarted) { Destroy(this, 2.0f); return; }

        if (Server_Manager.instance == null) {
            //Debug.Log("WTF");
            if (GameObject.FindObjectOfType<Server_Manager>()) {
                GameObject.FindObjectOfType<Server_Manager>().SetInstance(); Debug.Log("Set Instance:1");
                if (Server_Manager.instance == null) {
                    if (GameObject.Find("Server")) { GameObject.Find("Server").GetComponent<Server_Manager>().SetInstance(); Debug.Log("Set Instance:2"); }
                }
            }
        }
        //if (Server_Manager.instance) { serverStarted = true; }
    }
}
