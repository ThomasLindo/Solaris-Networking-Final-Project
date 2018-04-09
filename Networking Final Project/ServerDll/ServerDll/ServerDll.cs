using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ServerDll {

    /// <summary> Client </summary>
    public class Client  {

        #region Agent Variables & Functions
        //===========================//
        //Agent Variables & Functions//
        //===========================//
        /// <summary> TCP: Handle Incoming Data </summary>
        public string TCP_GetData() {
            string data = TCP_OnIncomingData();
            if (string.IsNullOrEmpty(data)) { return ""; }
            
            return (data);
        }
        /// <summary> TCP: Handle Outgoing Data </summary>
        public void TCP_SendData(string data) {
            if (data.Contains("?Heal")) {
                //PlayerManager.instance.GetComponent<PlayerStats>().FullHealth();
                //return;
            }

            //Send data to the Server
            TCP_Send(data);
        }


        #endregion

        #region Text Box Variables & Functions
        //==============================//
        //Text Box Variables & Functions//
        //==============================//

        public GameObject chatContainer;
        public GameObject messagePrefab;
        private int ChatWidth = 24;
        private int ChatBaseHeight = 20;

        /// <summary> TCP: Broken; Needs to Instantiate a (messagePrefab) </summary>
        public string TCP_InstantiateData() {
            string data = TCP_OnIncomingData();
            if (string.IsNullOrEmpty(data)) { return ""; }

            //GameObject g = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
            //g.GetComponentInChildren<Text>().text = data;
            //Vector2 vec2 = g.GetComponent<RectTransform>().sizeDelta;
            //vec2.y = (vec2.y * (Mathf.Ceil(data.Length / ChatWidth) + 1));
            //g.GetComponent<RectTransform>().sizeDelta = vec2;
            //chatContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContainer.GetComponent<RectTransform>().sizeDelta.x, chatContainer.GetComponent<RectTransform>().sizeDelta.y + vec2.y);

            return (data);
        }
        /// <summary> TCP: Set [input GameObject] to incomming text </summary>
        public string TCP_InstantiateData(GameObject g) {
            string data = TCP_OnIncomingData();
            if (string.IsNullOrEmpty(data)) { return ""; }

            g.GetComponentInChildren<Text>().text = data;
            Vector2 vec2 = g.GetComponent<RectTransform>().sizeDelta;
            vec2.y = (vec2.y * (Mathf.Ceil(data.Length / ChatWidth) + 1));
            g.GetComponent<RectTransform>().sizeDelta = vec2;
            chatContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(chatContainer.GetComponent<RectTransform>().sizeDelta.x, chatContainer.GetComponent<RectTransform>().sizeDelta.y + vec2.y);

            return (data);
        }
        /// <summary> TCP: Send Text that is in an [InputField] on an object named [ChatInput] </summary>
        public void TCP_OnSendButton() {
            InputField input = GameObject.Find("ChatInput").GetComponent<InputField>();
            if (!string.IsNullOrEmpty(input.text)) {
                TCP_Send(input.text);
                input.text = "";
            }
        }

        #endregion


        #region Basic Variables & Functions
        //===========================//
        //Basic Variables & Functions//
        //===========================//

        private string IP = "127.0.0.1"; //Internet Protocol Address
        private int Port = 8888; //Port Number 
        private string clientName = "Player"; //Name of the Client this is attached to.
        /// <summary> Set Internet Protocol Address </summary>
        public void SetIP(string ip) { IP = ip; }
        /// <summary> Set Port Number </summary>
        public void SetPort(int port) { Port = port; }
        /// <summary> Set name of the Client </summary>
        public void SetName(string name) { clientName = name; }
        /// <summary> Does Nothing </summary>
        public void Update() { }

        #endregion

        #region Basic TCP Variables & Functions
        //===============================//
        //Basic TCP Variables & Functions//
        //===============================//

        public bool TcpSocketReady = false; //Is the TCP socket avalible
        private TcpClient TcpSocket;
        private NetworkStream TcpStream;
        private StreamWriter TcpWriter;
        private StreamReader TcpReader;
        /// <summary> Try and Connect to a TCP Server </summary>
        public bool TCP_ConnectToServer() {
            if (TcpSocketReady) { return false; }

            //IP address
            if (GameObject.Find("IPInput")) {
                string h;
                h = GameObject.Find("IPInput").GetComponent<InputField>().text;
                if (h != null && h != "") { IP = h; }
            }
            //Port Number
            if (GameObject.Find("PortInput")) {
                int p = 0;
                int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
                if (p > 0 && p != 0) { Port = p; }
            }
            //Name
            if (GameObject.Find("PortInput")) {
                string n;
                n = GameObject.Find("IDInput").GetComponent<InputField>().text;
                if (n != null && n != "") { clientName = n; }
            }

            try {
                TcpSocket = new TcpClient(IP, Port);
                TcpStream = TcpSocket.GetStream();
                TcpWriter = new StreamWriter(TcpStream); ;
                TcpReader = new StreamReader(TcpStream);
                TcpSocketReady = true;
                return true;
            }
            catch (Exception e) { Debug.Log("TCP Client: Socket Error: " + e.Message); return false; }
        }
        /// <summary> Handle Incoming TCP Data </summary>
        public string TCP_OnIncomingData(){
            //Proceed if there is a working socket
            if (TcpSocketReady) {
                //Proceed if there is a data coming into the client
                if (TcpStream.DataAvailable) {
                    //Get data
                    string data = TcpReader.ReadLine();
                    //Proceed if there is a data
                    if (data != null) {
                        //Send out Clients name to the Server
                        if (data == "%NAME") {
                            TCP_Send("&NAME|" + clientName);
                            Debug.Log("TCP Client: Connected");
                            return "";
                        }
                        //Other Stuff
                        //Debug.Log("TCP Client: [" + data + "]");
                        return data;
                    }
                }
            }
            return "";
        }
        /// <summary> Send data to the TCP Server </summary>
        public void TCP_Send(string data) {
            if (!TcpSocketReady) { return; }
            else {
                TcpWriter.WriteLine(data);
                TcpWriter.Flush();
            }
        }
        /// <summary> Close Client TCP Socket; use in [OnApplicationQuit, OnDisable] </summary>
        public void TCP_CloseSocket() {
            if (!TcpSocketReady) { return; }

            TcpWriter.Close();
            TcpReader.Close();
            TcpSocket.Close();
            TcpSocketReady = false;
        }

        #endregion

        #region Basic UDP Variables & Functions
        //===============================//
        //Basic UDP Variables & Functions//
        //===============================//

        public bool UdpSocketReady = false; //Is the UDP socket avalible
        private UdpClient UdpSocket;
        private IPEndPoint ServerIPA = null;
        public List<string> Stuff = new List<string>();
        /// <summary> Try and Connect to a UDP Server </summary>
        public bool UDP_ConnectToServer() {
            if (UdpSocketReady) { return false; }

            //IP address
            if (GameObject.Find("IPInput")) {
                string h;
                h = GameObject.Find("IPInput").GetComponent<InputField>().text;
                if (h != null && h != "") { IP = h; }
            }
            //Port Number
            if (GameObject.Find("PortInput")) {
                int p = 0;
                int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
                if (p > 0 && p != 0) { Port = p; }
            }
            //Name
            if (GameObject.Find("PortInput")) {
                string n;
                n = GameObject.Find("IDInput").GetComponent<InputField>().text;
                if (n != null && n != "") { clientName = n; }
            }

            try {
                //UdpSocket = new UdpClient(Port);
                UdpSocket = new UdpClient(IP, Port);
                //UdpSocket.BeginReceive(UDP_AcceptClient, null);
                //Debug.Log("UDP Client: Where am I? : -2");
                UDP_StartListening();

                UdpSocketReady = true;
                return true;
            }
            catch (Exception e) { Debug.Log("UDP Client: Socket Error: " + e.Message); return false; }
        }
        /// <summary> Handle Incoming UDP Data </summary>
        //public string UDP_OnIncomingData(){
        //    //Proceed if there is a working socket
        //    if (UdpSocketReady) {
        //        //Proceed if there is a data coming into the client
        //        if (UdpStream.DataAvailable) {
        //            //Get data
        //            string data = UdpReader.ReadLine();
        //            //Proceed if there is a data
        //            if (data != null) {
        //                //Send out Clients name to the Server
        //                if (data == "%NAME") {
        //                    TCP_Send("&NAME|" + clientName);
        //                    return "";
        //                }
        //                //Other Stuff
        //                Debug.Log("[" + data + "]");
        //                return data;
        //            }
        //        }
        //    }
        //    return "";
        //}
        /// <summary> Send data to the UDP Server </summary>
        //public void UDP_Send(string data) {
        //    if (!UdpSocketReady) { return; }
        //    else {
        //        UdpWriter.WriteLine(data);
        //        UdpWriter.Flush();
        //    }
        //}
        /// <summary> Close Client UDP Socket; use in [OnApplicationQuit, OnDisable] </summary>
        public void UDP_CloseSocket() {
            if (!UdpSocketReady) { return; }
            UdpSocket.Close();
            UdpSocketReady = false;
        }

        /// <summary> Function handaler for UDP_AcceptClient  </summary>
        public void UDP_StartListening(IPAddress ip = null)  {
            //Debug.Log("UDP Client: Where am I? : -1");
            //UdpSocket.BeginReceive(UDP_AcceptClient, null);
            UdpSocket.BeginReceive(UDP_AcceptClient, UdpSocket);
        }
        /// <summary> Adds new Clients to the server & handles the received data </summary>
        public void UDP_AcceptClient(IAsyncResult ar) {
            //Debug.Log("UDP Client: Where am I? : 0");
            try {
                //Debug.Log("UDP Client: Where am I? : 1");
                IPEndPoint ipEndpoint = null;
                if (ServerIPA != null) { ipEndpoint = ServerIPA; }
                
                byte[] data = UdpSocket.EndReceive(ar, ref ipEndpoint);
                string message = System.Text.Encoding.UTF8.GetString(data);

                if (ServerIPA != ipEndpoint) {
                    ServerIPA = ipEndpoint;
                    UDP_Send("%NAME", ServerIPA);
                    Debug.Log("UDP Client: Connected to [" + ServerIPA.Address.ToString() + "]");
                }
                else {
                    //Debug.Log("UDP Client: Received [" + message + "]");
                    Stuff.Add(message);
                    //UDP_Send(message);
                }
            }
            catch (SocketException e) {
                // This happens when a client disconnects, as we fail to send to that port.
                Debug.Log("UDP Client: Disconnected");
            }
            UdpSocket.BeginReceive(UDP_AcceptClient, null);
        }
        /// <summary> Send data to a UDP Client </summary>
        public void UDP_Send(string message, IPEndPoint ipEndpoint) {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            UdpSocket.Send(data, data.Length, ipEndpoint);
        }
        /// <summary> Send data to a UDP Client </summary>
        public void UDP_Send(string message) {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            UdpSocket.Send(data, data.Length, ServerIPA);
        }
        
        /// <summary> UDP: Handle Incoming Data </summary>
        public string UDP_GetData() {
            //string data = UDP_OnIncomingData();

            if (Stuff.Count <= 0) { return ""; }

            string data = Stuff[Stuff.Count-1];
            if (string.IsNullOrEmpty(data)) { return ""; }

            //Do stuff with data
            Stuff.RemoveAt(Stuff.Count - 1);

            return (data);
        }
        /// <summary> UDP: Handle Outgoing Data </summary>
        public void UDP_SendData(string data) {
            if (data.Contains("?Heal")) {
                //PlayerManager.instance.GetComponent<PlayerStats>().FullHealth();
                return;
            }

            //Send data to the Server
            UDP_Send(data);
        }


        #endregion

    }

    /// <summary> Server </summary>
    public class Server {
        

        #region Basic Variables & Functions
        //===========================//
        //Basic Variables & Functions//
        //===========================//
        public List<ServerClient> TCP_Clients;
        private List<ServerClient> TCP_DisconnectList;
        private List<ServerClient> UDP_Clients;
        private List<ServerClient> UDP_DisconnectList;
        public int Port = 8888;
        public void SetPort(int port) { Port = port; }

        public void CheckIfDisConnected() {
            if (TcpServerStarted) {
                foreach (ServerClient c in TCP_Clients) {
                    if (!TCP_IsConnected(c.tcp)) {
                        c.tcp.Close();
                        TCP_DisconnectList.Add(c);
                    }
                }
                for (int i = 0; i < TCP_DisconnectList.Count-1; i++) {
                    TCP_Broadcast("TCP Server: " + TCP_DisconnectList[i].ClientName + " has disconnected.", TCP_Clients);
                    TCP_Clients.Remove(TCP_DisconnectList[i]);
                    TCP_DisconnectList.RemoveAt(i);
                }
            }
            if (UdpServerStarted) {
                foreach (ServerClient c in UDP_Clients) {
                    if (!UDP_IsConnected(c.udp)) {
                        c.udp.Close();
                        UDP_DisconnectList.Add(c);
                    }
                }
                for (int i = 0; i < UDP_DisconnectList.Count-1; i++) {
                    UDP_Broadcast("UDP Server: " + UDP_DisconnectList[i].ClientName + " has disconnected.", UDP_Clients);
                    UDP_Clients.Remove(UDP_DisconnectList[i]);
                    UDP_DisconnectList.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Basic TCP Variables & Functions
        //===============================//
        //Basic TCP Variables & Functions//
        //===============================//

        private TcpListener TcpServer;
        private bool TcpServerStarted = false;
        public bool TCP_Start()  {
            TCP_Clients = new List<ServerClient>();
            TCP_DisconnectList = new List<ServerClient>();

            try {
                TcpServer = new TcpListener(IPAddress.Any, Port);
                TcpServer.Start();
                TcpServerStarted = true;
                TCP_StartListening();
                Debug.Log("TCP Server [" + IPAddress.Any.ToString() + "] has been started on port [" + (Port).ToString() + "]");
                return true;
            } catch (Exception e) { Debug.Log("Socket Error" + e.Message); }
            return false;
        }
        public void TCP_Update() {
            if (TcpServerStarted) {
                foreach (ServerClient c in TCP_Clients) {
                    if (!TCP_IsConnected(c.tcp)) {
                        c.tcp.Close();
                        TCP_DisconnectList.Add(c);
                        continue; //don't have to do this
                    } else {
                        //Debug.Log(c.ClientName + " has connected");
                        NetworkStream s = c.tcp.GetStream();
                        if (s.DataAvailable) {
                            StreamReader reader = new StreamReader(s, true);
                            string data = reader.ReadLine();
                            if (!string.IsNullOrEmpty(data)) { TCP_OnIncomingData(c, data); }
                        }
                    }
                }
                for(int i = 0; i < TCP_DisconnectList.Count-1; i++) {
                    TCP_Broadcast("Server: " + TCP_DisconnectList[i].ClientName + " has disconnected.", TCP_Clients);
                    TCP_Clients.Remove(TCP_DisconnectList[i]);
                    TCP_DisconnectList.RemoveAt(i);
                }
            }
        }
        public void TCP_UpdateV2() {
            if (TcpServerStarted) {
                foreach (ServerClient c in TCP_Clients) {
                    if (!TCP_IsConnected(c.tcp)) {
                        c.tcp.Close();
                        TCP_DisconnectList.Add(c);
                        continue; //don't have to do this
                    } 
                }
                for(int i = 0; i < TCP_DisconnectList.Count-1; i++) {
                    TCP_Broadcast("Server: " + TCP_DisconnectList[i].ClientName + " has disconnected.", TCP_Clients);
                    TCP_Clients.Remove(TCP_DisconnectList[i]);
                    TCP_DisconnectList.RemoveAt(i);
                }
            }
        }
        public string TCP_GetDataV2(ServerClient c) {
            NetworkStream s = c.tcp.GetStream();
            if (s.DataAvailable) {
                StreamReader reader = new StreamReader(s, true);
                string data = reader.ReadLine();
                if (!string.IsNullOrEmpty(data)) {
                    if (data.Contains("&NAME")) {
                        c.ClientName = data.Split('|')[1];
                        TCP_Broadcast("TCP Server: " + c.ClientName + " has connected.", TCP_Clients);
                        return ("TCP Server: " + c.ClientName + " has connected.");
                    }
                    return data;
                }
            }
            return "";
        }


        /// <summary> Checks to see if the Clients are connected </summary>
        private bool TCP_IsConnected(TcpClient c) {
            try {
                if (c != null && c.Client != null && c.Client.Connected) {
                    if (c.Client.Poll(0, SelectMode.SelectRead)) {
                        return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                    }
                    return true;
                }
                else { return false; }
            }
            catch { return false; }
            
        }
        /// <summary> Handle Incoming TCP Data </summary>
        private void TCP_OnIncomingData(ServerClient c, string data) {
            if (data.Contains("&NAME")) {
                c.ClientName = data.Split('|')[1];
                TCP_Broadcast("TCP Server: " + c.ClientName + " has connected.", TCP_Clients);
                return;
            }
            //Send data to every client
            TCP_Broadcast(data, TCP_Clients);

            //Debug.Log("TCP Server: [" + c.ClientName + "] sent [" + data + "]");
        }
        /// <summary> Send data to the TCP Clients </summary>
        private void TCP_Broadcast(string data, List<ServerClient> cl) {
            foreach (ServerClient c in cl) {
                try {
                    StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                    writer.WriteLine(data);
                    writer.Flush();
                }
                catch (Exception e){
                    Debug.Log("TCP Server: Write Error: " + e.Message + " to client: " + c.ClientName);
                }
            }
        }
        /// <summary> Send data to the TCP Clients </summary>
        public void TCP_Send(string data) {
            foreach (ServerClient c in TCP_Clients) {
                try {
                    StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                    writer.WriteLine(data);
                    writer.Flush();
                }
                catch (Exception e){
                    Debug.Log("TCP Server: Write Error: " + e.Message + " to client: " + c.ClientName);
                }
            }
        }
        /// <summary> Function handaler for TCP_AcceptClient </summary>
        private void TCP_StartListening() {
            TcpServer.BeginAcceptSocket(TCP_AcceptClient, TcpServer);
        }
        /// <summary> Adds new Clients to the server </summary>
        private void TCP_AcceptClient(IAsyncResult ar) {
            TcpListener listener = (TcpListener)ar.AsyncState;

            TCP_Clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
            TCP_StartListening();

            TCP_Broadcast("%NAME", new List<ServerClient>() { TCP_Clients[TCP_Clients.Count-1] });
        }

        public void TCP_Close() {
            TcpServer.Stop();
            TCP_Clients.Clear();
            TCP_DisconnectList.Clear();
            TcpServerStarted = false;
        }

        #endregion

        #region Basic UDP Variables & Functions
        //===============================//
        //Basic UDP Variables & Functions//
        //===============================//
        private UdpClient UDPServer;
        private bool UdpServerStarted = false;
        
        public bool UDP_Start()  {
            UDP_Clients = new List<ServerClient>();
            UDP_DisconnectList = new List<ServerClient>();

            try {
                UDPServer = new UdpClient(Port);
                UdpServerStarted = true;
                UDP_StartListening();
                Debug.Log("UDP Server [" + IPAddress.Any.ToString() + "] has been started on port [" + Port.ToString() + "]");
                return true;

            } catch (Exception e) { Debug.Log("Socket Error" + e.Message); }
            return false;
        }
        public void UDP_Update() {
            if (UdpServerStarted) {
                foreach (ServerClient c in UDP_Clients) {
                    if (!UDP_IsConnected(c.udp)) {
                        c.udp.Close();
                        UDP_DisconnectList.Add(c);
                        continue; //don't have to do this
                    } else {
                        //Debug.Log(c.ClientName + " has connected");
                        UDPServer.BeginReceive(UDP_Data, UDPServer);
                        if (!string.IsNullOrEmpty(DataMessage)) {
                            UDP_OnIncomingData(c, DataMessage);
                            DataMessage = "";
                        }
                    }
                }
                for(int i = 0; i < UDP_DisconnectList.Count-1; i++) {
                    UDP_Broadcast("Server: " + UDP_DisconnectList[i].ClientName + " has disconnected.", UDP_Clients);
                    UDP_Clients.Remove(UDP_DisconnectList[i]);
                    UDP_DisconnectList.RemoveAt(i);
                }
            }
        }
        /// <summary>  </summary>
        private bool UDP_IsConnected(UdpClient c) {
            try {
                if (c != null && c.Client != null && c.Client.Connected) {
                    if (c.Client.Poll(0, SelectMode.SelectRead)) {
                        return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                    }
                    return true;
                }
                else { return false; }
            }
            catch { return false; }
            
        }
        /// <summary> Handle Incoming UDP Data </summary>
        private void UDP_OnIncomingData(ServerClient c, string data) {
            if (data.Contains("&NAME")) {
                c.ClientName = data.Split('|')[1];
                UDP_Broadcast("UDP Server: " + c.ClientName + " has connected.", UDP_Clients);
                return;
            }
            UDP_Broadcast(c.ClientName + ": " + data, UDP_Clients);
    
            //Debug.Log("UDP Server: " + c.ClientName + " sent [" + data + "]");
        }
        /// <summary> Send data to the UDP Clients </summary>
        private void UDP_Broadcast(string data, List<ServerClient> cl) {
            foreach (ServerClient c in cl) {
                try {
                    UDP_Send(data, c.SenderIPA);
                }
                catch (Exception e){
                    Debug.Log("UDP Server: Write Error: " + e.Message + " to client: " + c.ClientName);
                }
            }
        }
        /// <summary> Function handaler for UDP_AcceptClient </summary>
        private void UDP_StartListening(IPAddress ip = null)  {
            //UDPServer.BeginReceive(UDP_AcceptClient, null);
            UDPServer.BeginReceive(UDP_AcceptClient, UDPServer);
        }
        /// <summary> Adds new Clients to the server </summary>
        private void UDP_AcceptClient(IAsyncResult ar) {
            try {
                IPEndPoint ipEndpoint = null;
                byte[] data = UDPServer.EndReceive(ar, ref ipEndpoint);
                string message = System.Text.Encoding.UTF8.GetString(data);

                bool AlreadyConnected = false;
                for (int i = 0; i < UDP_Clients.Count; i++) {
                    if (UDP_Clients[i].SenderIPA == ipEndpoint) { AlreadyConnected = true; break; }
                }
                if (!AlreadyConnected) {
                    UDP_Clients.Add(new ServerClient((UdpClient)ar.AsyncState, ipEndpoint));
                    //UDP_Clients.Add(new ServerClient(ipEndpoint));
                    UDP_Broadcast("%NAME", new List<ServerClient>() { UDP_Clients[UDP_Clients.Count - 1] });
                    Debug.Log("UDP Server: [" + UDP_Clients[UDP_Clients.Count - 1].ClientName + "] connected [" + ipEndpoint + "]");
                }
                else {
                    UDP_Broadcast(message, new List<ServerClient>() { UDP_Clients[UDP_Clients.Count - 1] });
                    //Debug.Log("UDP Server: [" + message + "]");
                }
            }
            catch (SocketException e) {
                // This happens when a client disconnects, as we fail to send to that port.
                Debug.Log("UDP Server: Disconnected");
            }
            UDPServer.BeginReceive(UDP_AcceptClient, null);
        }
        /// <summary> Send data to a UDP Client </summary>
        public void UDP_Send(string message, IPEndPoint ipEndpoint) {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            UDPServer.Send(data, data.Length, ipEndpoint);
        }
        /// <summary>  </summary>
        private void UDP_RemoveClient(IPEndPoint ipEndpoint) {
            for (int i = 0; i < UDP_Clients.Count; i++) {
                if (UDP_Clients[i].SenderIPA == ipEndpoint) {
                    UDP_Clients[i].tcp.Close();
                    UDP_DisconnectList.Add(UDP_Clients[i]);
                }
            }
            for (int i = 0; i < UDP_DisconnectList.Count - 1; i++) {
                TCP_Broadcast("UDP Server: " + UDP_DisconnectList[i].ClientName + " has disconnected.", UDP_Clients);
                UDP_Clients.Remove(UDP_DisconnectList[i]);
                UDP_DisconnectList.RemoveAt(i);
            }
        }
        /// <summary> Remove a UDP Client </summary>
        public void UDP_Close() { UDPServer.Close(); }

        private string DataMessage = "";
        /// <summary>  </summary>
        private void UDP_Data(IAsyncResult ar) {
            try {
                IPEndPoint ipEndpoint = null;
                byte[] data = UDPServer.EndReceive(ar, ref ipEndpoint);
                DataMessage = System.Text.Encoding.UTF8.GetString(data);
            }
            catch (SocketException e) { }
        }

        #endregion

    }

    /// <summary> Server uses as Index of Clients that have connected </summary>
    public class ServerClient {
        public string ClientName;
        public TcpClient tcp;
        public UdpClient udp;
        public IPEndPoint SenderIPA;
        //
        public ServerClient(TcpClient TcpClientSocket) {
            ClientName = "Guest";
            tcp = TcpClientSocket;
        }
        public ServerClient(UdpClient UdpClientSocket) {
            ClientName = "Guest";
            udp = UdpClientSocket;
        }
        public ServerClient(TcpClient TcpClientSocket, UdpClient UdpClientSocket) {
            ClientName = "Guest";
            tcp = TcpClientSocket;
            udp = UdpClientSocket;
        }
        public ServerClient(UdpClient UdpClientSocket, TcpClient TcpClientSocket) {
            ClientName = "Guest";
            tcp = TcpClientSocket;
            udp = UdpClientSocket;
        }


        public ServerClient(IPEndPoint ipEndpoint) {
            ClientName = "Guest";
            udp = new UdpClient();
            SenderIPA = ipEndpoint;
        }
        public ServerClient(UdpClient UdpClientSocket, IPEndPoint ipEndpoint) {
            ClientName = "Guest";
            udp = UdpClientSocket;
            SenderIPA = ipEndpoint;
        }
    }
}
