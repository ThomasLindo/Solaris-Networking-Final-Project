using UnityEngine;

public class Load_Menu : MonoBehaviour {

    GameObject EmptyWorld;
    public GameObject Menu_Prefab;
    public GameObject Items_Prefab;
    public GameObject Player_Prefab;
    //GameObject Inventory = (GameObject)Instantiate(Resources.Load("MyPrefab"));


    // Use this for initialization
    void Start () {
        //Spawns an empty gameObject named [World]
        EmptyWorld = new GameObject("World");

        //Spawns Prefab used for the Menu
        GameObject Menu = (GameObject)Instantiate(Menu_Prefab);
        Menu.transform.SetParent(EmptyWorld.transform);
        Menu.name = Menu_Prefab.name;

        //Spawns Prefab used for the Items on the floor
        GameObject Items = (GameObject)Instantiate(Items_Prefab);
        Items.transform.SetParent(EmptyWorld.transform);
        Items.name = Items_Prefab.name;

        //Spawns Prefab used for the Player
        GameObject Player = (GameObject)Instantiate(Player_Prefab);
        Player.transform.SetParent(EmptyWorld.transform);
        Player.name = Player_Prefab.name;
    }
}
