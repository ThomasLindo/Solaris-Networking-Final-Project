using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;
    void Awake() { instance = this; }
    public GameObject player;


    public bool MenuOpen = false;
    public int TriggerHit = 0;
    private float MenuTimer = 0.0f;
    private MenuManager menuManager;

    private GameObject InventorySlot;
    private Color color = UnityEngine.Color.white;

    public AudioSource PickupSound;

    private void Start() {
        Invoke("DelayedStart", 0.1f);
        InvokeRepeating("UpdateSlow", 1.0f, 1.0f);
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() {
        menuManager = MenuManager.instance;
        InventorySlot = menuManager.Inventory_Slot.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (!MenuOpen && MenuTimer > 0.0f) { MenuTimer -= Time.deltaTime; return; }
        else if (!MenuOpen && MenuTimer < 0.0f) { MenuTimer = 0.0f; }
        else if (MenuOpen && MenuTimer > 0.0f) { MenuTimer -= Time.fixedUnscaledDeltaTime; return; }
        else if (MenuOpen && MenuTimer < 0.0f) { MenuTimer = 0.0f; }

        if (player.GetComponent<PlayerStats>().isDead) { ButtonManager.instance.OpenDeathMenu(); return; }
        isColliding = false;


        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) {
            //If player is in a menu exit it
            if (ExitAllMenus()){ return; }
            MenuTimer = 0.1f;
            MenuOpen = true;
            //Enter pause menu
            if (ButtonManager.instance) { ButtonManager.instance.OpenPauseMenu(); }
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetButtonDown("Back")) {
            //If player is in a menu exit it
            if (ExitAllMenus()) { return; }
            MenuTimer = 0.1f;
            MenuOpen = true;
            //Enter score menu
            if (ButtonManager.instance) { ButtonManager.instance.OpenScoreMenu(); }
        }
        else if (Input.GetButtonDown("Tab") || Input.GetButtonDown("CY")) { ExitMenus(); }
        else if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("RightStickDown")) {
            MenuTimer = 0.1f;
            if (this.transform.Find("Flash_Light")){
                this.transform.Find("Flash_Light").GetComponent<SwitchActive>().Switch();
            }
        }
        else if (Input.GetKeyDown(KeyCode.T)) {
            return; //Do nothing
            Transform temp = GameObject.Find("Managers").transform.Find("ChatServerDll");
            if (temp){
                temp.GetComponent<SwitchActive>().Switch();
                if (temp.GetComponent<SwitchActive>().isActive) {
                    Cursor.lockState = CursorLockMode.None;
                    MenuOpen = true;
                }
                else {
                    ExitAllMenus();
                }
            }
        }
    }

    private void UpdateSlow() {
        if (player.GetComponent<PlayerStats>().isDead) { ButtonManager.instance.OpenDeathMenu(); return; }


        if (menuManager.Ammo_Slot.GetComponent<Ammo_Inventory>().CombindStacks()) {
            menuManager.Ammo_Slot.GetComponent<Drop_Inventory>().NumberOfSlotsFilled = menuManager.Ammo_Slot.transform.childCount - 1; //has a chance that and additional ammo can be added into the inventory
        }
    }

    public void KillPlayer() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool isColliding = false;

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Items" && other.gameObject.layer == 2) { TriggerHit++; }
        else if (other.tag == "Items" && other.GetComponent<Item_To_Pick_Up>()) {
            if (isColliding) { return; }
            else { isColliding = true; }

            GameObject ItemPickUp = other.GetComponent<Item_To_Pick_Up>().PickUpItem;
            if (ItemPickUp.GetComponent<Drag_Inventory>() && ItemPickUp.GetComponent<Drag_Inventory>().typeOfItem == Drag_Inventory.Slot.Ammo) {
                Drop_Inventory DropA = menuManager.Ammo_Slot.GetComponent<Drop_Inventory>();
                //If the player has room in their ammo inventory, pick up the item
                if (DropA.NumberOfSlotsFilled < DropA.NumberOfSlotsTotal) {
                    //MenuTimer = 0.05f;
                    //Spawn item
                    GameObject Item = (GameObject)Instantiate(ItemPickUp);
                    Item.name = ItemPickUp.name;
                    Item.GetComponent<Ammo_Types>().Amount = other.GetComponent<Ammo_Types>().Amount;
                    //Set its parent to the ammo inventory
                    Item.transform.SetParent(DropA.transform);
                    //Sets default scale
                    Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //Incresses slots filled count
                    if (menuManager.Ammo_Slot.GetComponent<Ammo_Inventory>().CombindStacks()) {
                        DropA.NumberOfSlotsFilled = menuManager.Ammo_Slot.transform.childCount - 1; //has a chance that and additional ammo can be added into the inventory
                    }
                    Destroy(other.gameObject);

                    if (PickupSound != null) { PickupSound.Play(); }
                }
            }
        }
    }
    private void OnTriggerStay(Collider other) {

        //If player is in a menu exit it
        if (MenuTimer > 0.0f) { return; }
        if (MenuOpen && other.tag == "Check_Tag" && (Input.GetButton("Tab") || Input.GetButton("CY"))) { CloseCheckTag(other); return; }

        //If player is selecting a 
        if (Input.GetButtonDown("E") || Input.GetButtonDown("CB")) {
            //If player interacts with an item
            if (other.tag == "Items") { ItemPickUp(other); }
        }
        else if (Input.GetButtonDown("Tab") || Input.GetButtonDown("CY")) {
            //If player interacts with a Computer
            if (other.tag == "Check_Tag") { OpenCheckTag(other); }
            //If player interacts with a crafting table
            else if (other.tag == "Crafting_Table") { CraftingMenu(other); }
            //If player interacts with a NPC
            else if (other.tag == "NPC") { }
            //Enter drop menu
            else if (TriggerHit == 0) {
                if (MenuOpen) { CloseDropMenu(); }
                else { OpenDropMenu(); }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag != "Items" && other.gameObject.layer == 2) { TriggerHit--; }
        //If player interacts with a crafting table
        if (other.tag == "Crafting_Table") { Crafting_Table.instance.CloseCraftingTable(); }
        CloseCheckTag(other);
    }


    private void OpenCheckTag(Collider other) {
        if (other.gameObject.name == "Computer") {
            other.gameObject.GetComponent<SwitchOn>().SwitchON();
            Cursor.lockState = CursorLockMode.None;
            MenuTimer = 0.1f;
            MenuOpen = true;
            Time.timeScale = 0.10f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }
    private void CloseCheckTag(Collider other) {
        if (other.gameObject.name == "Computer") {
            other.gameObject.GetComponent<SwitchOn>().SwitchOFF();
            Cursor.lockState = CursorLockMode.Locked;
            MenuTimer = 0.1f;
            MenuOpen = false;
            Time.timeScale = 01.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    private void CraftingMenu(Collider other) {
        MenuTimer = 0.1f;
        //If already in the crafting menu then exit it
        if (other.gameObject.transform.GetChild(0) && other.gameObject.transform.GetChild(0).gameObject.activeSelf) { ExitMenus(); }
        else { if (Crafting_Table.instance) { Crafting_Table.instance.OpenCraftingTable(); } }
    }

    private void ItemPickUp(Collider other) {
        GameObject ItemPickUp = other.GetComponent<Item_To_Pick_Up>().PickUpItem;
        Drop_Inventory DropI = InventorySlot.GetComponent<Drop_Inventory>();

        if (ItemPickUp.GetComponent<Drag_Inventory>() && ItemPickUp.GetComponent<Drag_Inventory>().typeOfItem == Drag_Inventory.Slot.Ammo) {
            Drop_Inventory DropA = menuManager.Ammo_Slot.GetComponent<Drop_Inventory>();
            //If the player has room in their ammo inventory, pick up the item
            if (DropA.NumberOfSlotsFilled < DropA.NumberOfSlotsTotal) {
                MenuTimer = 0.05f;
                //Spawn item
                GameObject Item = (GameObject)Instantiate(ItemPickUp);
                Item.name = ItemPickUp.name;
                Item.GetComponent<Ammo_Types>().Amount = other.GetComponent<Ammo_Types>().Amount;
                //Set its parent to the ammo inventory
                Item.transform.SetParent(DropA.transform);
                //Sets default scale
                Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Incresses slots filled count
                if (menuManager.Ammo_Slot.GetComponent<Ammo_Inventory>().CombindStacks()) {
                    DropA.NumberOfSlotsFilled = menuManager.Ammo_Slot.transform.childCount - 1; //has a chance that and additional ammo can be added into the inventory
                }
                Destroy(other.gameObject);

                if (PickupSound != null) { PickupSound.Play(); }
            }
        }
        //If the player has room in their inventory, pick up the item
        else if (DropI.NumberOfSlotsFilled < DropI.NumberOfSlotsTotal && ItemPickUp.GetComponent<Drag_Inventory>().typeOfItem != Drag_Inventory.Slot.Ammo) {
            MenuTimer = 0.05f;
            //Spawn item
            GameObject Item = (GameObject)Instantiate(ItemPickUp);
            Item.name = ItemPickUp.name;
            //Set its parent to the inventory
            Item.transform.SetParent(DropI.transform);
            //Sets default scale
            Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //Incresses slots filled count
            DropI.NumberOfSlotsFilled++;

            if (Item.GetComponent<ItemStats>() && other.transform.GetChild(0).GetComponent<Gun_Behaviour>()) {
                //Debug.Log("Pickup : Before " + Item.GetComponent<ItemStats>().itemStats[4].baseValue + " After " + other.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats[4].baseValue);
                Item.GetComponent<ItemStats>().itemStats = other.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats;
            }

            //Destroy item on ground
            Destroy(other.gameObject);

            if (PickupSound != null) { PickupSound.Play(); }
        }
    }

    private void OpenDropMenu() {
        Cursor.lockState = CursorLockMode.None;
        MenuOpen = true;
        menuManager.EnableGraphicRaycaster(true);
        menuManager.DTF_Slot.transform.parent.GetComponent<GraphicRaycaster>().enabled = true;
        color.a = 0.20f;
        menuManager.DTF_Slot.GetComponent<Image>().color = color;
        MenuTimer = 0.1f;
    }
    public void CloseDropMenu() {
        Cursor.lockState = CursorLockMode.Locked;
        MenuOpen = false;
        menuManager.EnableGraphicRaycaster(false);
        menuManager.DTF_Slot.transform.parent.GetComponent<GraphicRaycaster>().enabled = false;
        color.a = 0.0f;
        menuManager.DTF_Slot.GetComponent<Image>().color = color;
        MenuTimer = 0.1f;

    }


    private void ExitMenus() {
        MenuTimer = 0.1f;
        //If a menu is open, close it
        CloseDropMenu();
        if (Crafting_Table.instance) { Crafting_Table.instance.CloseCraftingTable(); }
        if (ButtonManager.instance) { ButtonManager.instance.ClosePauseMenu(); }
        if (ButtonManager.instance) { ButtonManager.instance.CloseScoreMenu(); }
        Cursor.lockState = CursorLockMode.Locked;
        MenuOpen = false;
    }
    private bool ExitAllMenus() {
        if (MenuOpen) {
            MenuTimer = 0.1f;
            //If a menu is open, close it
            CloseDropMenu();
            if (Crafting_Table.instance) { Crafting_Table.instance.CloseCraftingTable(); }
            if (ButtonManager.instance) { ButtonManager.instance.ClosePauseMenu(); }
            if (ButtonManager.instance) { ButtonManager.instance.CloseScoreMenu(); }
            Cursor.lockState = CursorLockMode.Locked;
            MenuOpen = false;
            return true;
        }
        else { return false; }
    }

}