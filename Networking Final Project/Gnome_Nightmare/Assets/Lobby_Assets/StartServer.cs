using UnityEngine;

public class StartServer : MonoBehaviour {
    public Chat_Manager chat_Manager;

    private void Awake() {
        if (chat_Manager != null) { chat_Manager.EnableServer(); }
    }
}
