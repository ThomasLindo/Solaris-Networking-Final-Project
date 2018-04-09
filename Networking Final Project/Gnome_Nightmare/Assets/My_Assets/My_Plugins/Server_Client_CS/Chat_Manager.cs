using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChatSystem;
using Sirenix.OdinInspector;

public class Chat_Manager : SerializedMonoBehaviour {
    
    [ToggleGroup("IsServer", order: 1, groupTitle: "Server")]
    public bool IsServer;
    [ToggleGroup("IsServer")]
    private ChatSystem.Server server = new ChatSystem.Server();

    [ToggleGroup("IsClient", order: 1, groupTitle: "Client")]
    public bool IsClient;
    [ToggleGroup("IsClient")]
    private ChatSystem.Client client = new ChatSystem.Client();
    [ToggleGroup("IsClient")]
    public GameObject chatContainer;
    [ToggleGroup("IsClient")]
    public GameObject messagePrefab;

    [ToggleGroup("IsClient")]
    public GameObject Chat;
    [ToggleGroup("IsClient")]
    public GameObject Login;


    private bool ServerEnabled = false;

    

    // Use this for initialization
    public void Start () {
        //if (IsServer && ServerEnabled) { ServerStart(); }
        //if (IsClient) { ClientStart(); }
    }

    // Update is called once per frame
    public void Update () {
        if (IsServer && ServerEnabled) { ServerUpdate(); }
        if (IsClient) { ClientUpdate(); }
    }


    [ToggleGroup("IsServer")]
    public void EnableServer() { ServerEnabled = true; ServerStart(); }

    private void ServerStart() {
        server.Start();
    }
    private void ServerUpdate() {
        server.Update();
    }



    [ToggleGroup("IsClient")]
    private void ClientStart() {
        client.chatContainer = chatContainer;
        client.messagePrefab = messagePrefab;
    }
    private void ClientUpdate() {
        //if (chatContainer != null) { chatContainer = GameObject.Find("Content").gameObject; }
        if (chatContainer != null) { client.chatContainer = chatContainer; }
        if (messagePrefab != null) { client.messagePrefab = messagePrefab; }
        //client.Update();

        GameObject g = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
        string temp = client.InstantiateData(g);


        //string temp = client.InstantiateData();
        if (temp != null && temp != "") {
            //GameObject g = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
            //g.GetComponentInChildren<Text>().text = temp;
            //Vector2 vec2 = g.GetComponent<RectTransform>().sizeDelta;
            //vec2.y = (vec2.y * Mathf.Ceil((24 - client.clientName.Length + temp.Length) / 24));
            //g.GetComponent<RectTransform>().sizeDelta = vec2;
            Debug.Log(((24 - client.clientName.Length + temp.Length)) + "[" + temp + "]");
        }
        else { Destroy(g); }
        
    }
    [ToggleGroup("IsClient")]
    public void ConnectToServer() {
        if (client.ConnectToServer()) {
            if (Chat) { Chat.GetComponent<UnHide>().View(); }
            if (Login) { Login.GetComponent<UnHide>().Hide(); }
        }
        else { }
    }
    [ToggleGroup("IsClient")]
    public void OnSendButton() {
        client.OnSendButton();
    }

    public void OnApplicationQuit() {
        if (client != null) { client.OnApplicationQuit(); }
        //if (server != null) { server.OnApplicationQuit(); }
    }
}

