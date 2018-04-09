using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    public Dialogue NPCDialogue;
    public Output LoadText;
    public int TextNumber;
    public GameObject Dialogue_Prefab;
    
    private bool MenuOpen = false;

    private void Start() {
        LoadText = new Output();
        TextNumber = 0;
    }

    private void OnTriggerStay(Collider other) {
        //If the player collides with an NPC and presses "E" to interact
        if (other.tag == "NPC" && Input.GetButton("E") && MenuOpen == false) {
            MenuOpen = true;
            //If the Dialogue menu is allready open do nothing
            if (other.transform.Find("Dialogue_Menu") == true) {
                Debug.Log("It's already there.");
                DistoryNpcDialogue();
            }
            else {
                Cursor.lockState = CursorLockMode.None;
                PlayerManager.instance.MenuOpen = true;
                //Spawn a Dialogue Menu
                GameObject Dialogue_Menu = (GameObject)Instantiate(Dialogue_Prefab);
                //Set the Parent of the Dialogue Menu to the NPC
                Dialogue_Menu.transform.SetParent(other.transform);
                //Rename the Menu to "Dialogue_Menu" for better use later
                Dialogue_Menu.name = "Dialogue_Menu";
                //Load all the Dialogue that is associated to the NPC
                LoadAllNpcDialogue(other);
                //Set the UI to one of the Text files loaded
                SetUIDialogue(TextNumber);
                Dialogue_Menu.transform.Find("Dialogue_Box").transform.Find("Button").GetComponent<Button>().onClick.AddListener(ContinueDialogue);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        //When a player leaves the area of the NPC, destroy the Dialogue Menu
        DistoryNpcDialogue();
    }

    private void LoadAllNpcDialogue(Collider other) {
        //Sets the name to the name of the NPC
        //Will be used to find the Dialogue of the NPC
        NPCDialogue.NameOfNPC = other.name;
        int i = 0;

        string Base;
        if (System.Reflection.Assembly.GetExecutingAssembly().CodeBase != null) {
            Base = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            if (Base.Remove(0, Base.Length - 44) == "Library/ScriptAssemblies/Assembly-CSharp.dll") { Base = Base.Remove(Base.Length - 44, 44); }
            Base = Base.Remove(0, 8);
        }
        else { Base = ""; }
        Debug.Log(Base + "Assets/My_Assets/My_Dialogue/" + NPCDialogue.NameOfNPC);

        //Loops through and finds all the files associated with the NPC
        foreach (string file in System.IO.Directory.GetFiles(Base + "Assets/My_Assets/My_Dialogue/" + NPCDialogue.NameOfNPC)){
            //If the file is a .txt file then it will save the text in it
            if (file.Remove(0, file.Length - 4) == ".txt") {
                //Removes [Assets/My_Assets/My_Dialogue/] Which is 29 characters 
                //Removes the [NPC's name] followed by a [/]
                string tempFileName = file.Remove(0, 29 + NPCDialogue.NameOfNPC.Length + 1);
                //Removes the last 4 characters ie. [.txt]
                tempFileName = tempFileName.Remove(tempFileName.Length - 4);
                //Saves the name of the file
                NPCDialogue.FileName.Add(tempFileName);
                //Saves the Text in the file as the NPC's Dialogue
                NPCDialogue.sentences.Add(LoadText.ReadText(NPCDialogue.NameOfNPC, tempFileName));
                i++;
                NPCDialogue.NumberOfSentences = i;
            }
        }

    }

    private void SetUIDialogue(int Number) {
        //Finds and sets the UI text to the Dialogue loaded.
        //These exist with proper names from the prefab.
        GameObject.Find("Dialogue_Menu").transform.Find("Dialogue_Box").transform.Find("NPCName").GetComponent<Text>().text = NPCDialogue.NameOfNPC;
        GameObject.Find("Dialogue_Menu").transform.Find("Dialogue_Box").transform.Find("NPCDialogue").GetComponent<Text>().text = NPCDialogue.sentences[Number];
    }

    private void DistoryNpcDialogue() {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerManager.instance.MenuOpen = false;
        //Finds the Dialogue Menu and distroys it
        if (GameObject.Find("Dialogue_Menu") == true) {
            Destroy(GameObject.Find("Dialogue_Menu").gameObject);
            //Destroys the loaded text files associated with the NPC
            NPCDialogue.OnDestroy();
            MenuOpen = false;
            TextNumber = 0;
        }
    }

    public void ContinueDialogue() {
        //Finds the [Player] and accesses the [DialogueManager] that is attached to it
        DialogueManager Temp = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueManager>();
        //If there is Dialogue to load, load it.
        //Will be changed as just loading one after the other is stupid...
        //Will be changed to allow multiple Dialogue paths
        if (Temp.TextNumber < Temp.NPCDialogue.NumberOfSentences - 1) {
            Temp.TextNumber += 1;
            Temp.SetUIDialogue(Temp.TextNumber);
        }
        //No more Dialogue to load
        else { Temp.DistoryNpcDialogue(); }
    }
}
