using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEngine.UI;

public class GetIP : MonoBehaviour {

    public string IP = "";
    public Text text;

    void Start() {
        IP = GetIPAddress();
        SetText();
    }

    public string GetIPAddress() {
        return Network.player.ipAddress;
        //string strHostName = "";
        //strHostName = System.Net.Dns.GetHostName();
        //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
        //IPAddress []addr = ipEntry.AddressList;
        //return addr[addr.Length - 1].ToString();
    }
    public void SetText() {
        if (text) { text.text = IP; }
    }
    public void SetText(Text setTest) {
        if (setTest) { setTest.text = IP; }
    }
}
