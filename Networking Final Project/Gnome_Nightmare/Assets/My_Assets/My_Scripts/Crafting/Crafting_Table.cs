using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crafting_Table : SerializedMonoBehaviour {

    public static Crafting_Table instance;
    void Awake() { instance = this; }
    private CraftingManager craftingManager;
    private MenuManager menuManager;

    //[HideLabel]
    //[PreviewField(50, ObjectFieldAlignment.Left)]
    //public Object D;

    [FoldoutGroup("Combined Table")]
    [TableList]
    public List<OdinTables.Table3x5> CombinedTable = new List<OdinTables.Table3x5>();

    [FoldoutGroup("Modifier Table")]
    [TableList]
    public List<OdinTables.Table2x5> ModifierTable = new List<OdinTables.Table2x5>();
    

    [System.NonSerialized]
    public bool IsCrafting = false;
    [System.NonSerialized]
    public int OnSlot = 0;
    private float timer = 0.0f;
    private Color color = UnityEngine.Color.white;

    // Use this for initialization
    void Start() {
        Invoke("DelayedStart", 0.1f);
    }

    private void DelayedStart() {
        craftingManager = CraftingManager.instance;
        menuManager = MenuManager.instance;
    }

    // Update is called once per frame
    void Update() {
        if (menuManager == null) { menuManager = MenuManager.instance; }
        if (IsCrafting) {
            if (PlayerManager.instance.MenuOpen && timer > 0.0f) { timer -= Time.fixedUnscaledDeltaTime; }
            else if (!PlayerManager.instance.MenuOpen && timer > 0.0f) { timer -= Time.deltaTime; }
            LeftJoystickPlacement();
        }
    }

    public void OpenCraftingTable() {
        PlayerManager.instance.MenuOpen = true;
        menuManager.EnableGraphicRaycaster(true);
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(0).transform.GetChild(0).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        IsCrafting = true;
        //Unlock the cursor
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0.10f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void CloseCraftingTable() {
        PlayerManager.instance.MenuOpen = false;
        menuManager.EnableGraphicRaycaster(false);
        //Deactivate crafting table menu
        this.transform.GetChild(0).gameObject.SetActive(false);
        IsCrafting = false;
        //Lock cursor 
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 01.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }


    public void CheckModifiers() {
        if (craftingManager.Output_Slot.transform.childCount == 1 && craftingManager.Modifier_Slot.transform.childCount == 1) {
            Debug.Log("Modified");
            for (int j = 0; j < CombinedTable.Count; j++) {
                for (int i = 0; i < 5; i++) {
                    if (ModifierTable[j].table2x5[0, i] == null) {  }
                    else if (craftingManager.Modifier_Slot.transform.GetChild(0).name == ModifierTable[j].table2x5[0, i].name) {
                        ItemStats ItemInOut = craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>();
                        ItemStats ItemInMod = craftingManager.Modifier_Slot.transform.GetChild(0).GetComponent<ItemStats>();
                        ItemInOut.itemStats.AddAllModifiers(ItemInMod.itemStats, ItemInOut.itemStats);
                        ItemInOut.itemStats.SquishAllModifierValues(ItemInOut.itemStats);

                        Destroy(ItemInMod.gameObject);
                    }
                    else if (ModifierTable[j].table2x5[1, i] == null) {  }
                    else if (craftingManager.Modifier_Slot.transform.GetChild(0).name == ModifierTable[j].table2x5[1, i].name) {
                        ItemStats ItemInOut = craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>();
                        ItemStats ItemInMod = craftingManager.Modifier_Slot.transform.GetChild(0).GetComponent<ItemStats>();
                        ItemInOut.itemStats.AddAllModifiers(ItemInMod.itemStats, ItemInOut.itemStats);
                        ItemInOut.itemStats.SquishAllModifierValues(ItemInOut.itemStats);

                        Destroy(ItemInMod.gameObject);
                    }
                }
            }
        }
    }

    public void CheckCombinations() {
        if (craftingManager.Combined_First_Slot.transform.childCount == 1 && craftingManager.Combined_Second_Slot.transform.childCount == 1 && craftingManager.Output_Slot.transform.childCount == 0) {
            for (int j = 0; j < CombinedTable.Count; j++) {
                for (int i = 0; i < 5; i++) {
                    if (CombinedTable[j].table3x5[0, i] == null || CombinedTable[j].table3x5[1, i] == null || CombinedTable[j].table3x5[2, i] == null) { return; }

                    if ((craftingManager.Combined_First_Slot.transform.GetChild(0).name == CombinedTable[j].table3x5[0, i].name && craftingManager.Combined_Second_Slot.transform.GetChild(0).name == CombinedTable[j].table3x5[1, i].name)
                      || (craftingManager.Combined_First_Slot.transform.GetChild(0).name == CombinedTable[j].table3x5[1, i].name && craftingManager.Combined_Second_Slot.transform.GetChild(0).name == CombinedTable[j].table3x5[0, i].name)) {
                        Debug.Log("Assemble");
                        GameObject Item = (GameObject)Instantiate(CombinedTable[j].table3x5[2, i]);
                        Item.name = CombinedTable[j].table3x5[2, i].name;
                        Item.transform.SetParent(craftingManager.Output_Slot.transform);
                        Item.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        //Item.GetComponent<ItemStats>().Damage.AddModifier(craftingManager.Combined_First_Slot.transform.GetChild(0).GetComponent<ItemStats>().Damage.GetModifierValue() + craftingManager.Combined_Second_Slot.transform.GetChild(0).GetComponent<ItemStats>().Damage.GetModifierValue());
                        ItemStats ItemCom1 = craftingManager.Combined_First_Slot.transform.GetChild(0).GetComponent<ItemStats>();
                        ItemStats ItemCom2 = craftingManager.Combined_Second_Slot.transform.GetChild(0).GetComponent<ItemStats>();
                        Item.GetComponent<ItemStats>().itemStats.AddAllModifiers(ItemCom1.itemStats, Item.GetComponent<ItemStats>().itemStats);
                        Item.GetComponent<ItemStats>().itemStats.AddAllModifiers(ItemCom2.itemStats, Item.GetComponent<ItemStats>().itemStats);
                        Item.GetComponent<ItemStats>().itemStats.SquishAllModifierValues(Item.GetComponent<ItemStats>().itemStats);


                        Destroy(ItemCom1.gameObject);
                        Destroy(ItemCom2.gameObject);
                    }
                }
            }
        }
    }

    public void CheckDisassemble() {
        if (craftingManager.Output_Slot.transform.childCount == 1 && (craftingManager.Disassemble_First_Slot.transform.childCount == 0 && craftingManager.Disassemble_Second_Slot.transform.childCount == 0)) {
            for (int j = 0; j < CombinedTable.Count; j++) {
                for (int i = 0; i < 5; i++) {
                    if (CombinedTable[j].table3x5[0, i] == null || CombinedTable[j].table3x5[1, i] == null || CombinedTable[j].table3x5[2, i] == null) { return; }

                    if (craftingManager.Output_Slot.transform.GetChild(0).name == CombinedTable[j].table3x5[2, i].name) {
                        Debug.Log("Disassemble");
                        int RandomPercent = Random.Range(1, 9);

                        GameObject Item1 = (GameObject)Instantiate(CombinedTable[j].table3x5[0, i]);
                        Item1.name = CombinedTable[j].table3x5[0, i].name;
                        Item1.transform.SetParent(craftingManager.Disassemble_First_Slot.transform);
                        Item1.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        Item1.GetComponent<ItemStats>().itemStats.AddAllModifiersByPercent(craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>().itemStats, Item1.GetComponent<ItemStats>().itemStats, (1.0f - (RandomPercent * 0.1f)));
                        Item1.GetComponent<ItemStats>().itemStats.SquishAllModifierValues(Item1.GetComponent<ItemStats>().itemStats);

                        GameObject Item2 = (GameObject)Instantiate(CombinedTable[j].table3x5[1, i]);
                        Item2.name = CombinedTable[j].table3x5[1, i].name;
                        Item2.transform.SetParent(craftingManager.Disassemble_Second_Slot.transform);
                        Item2.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        Item2.GetComponent<ItemStats>().itemStats.AddAllModifiersByPercent(craftingManager.Output_Slot.transform.GetChild(0).GetComponent<ItemStats>().itemStats, Item1.GetComponent<ItemStats>().itemStats, (RandomPercent * 0.1f));
                        Item2.GetComponent<ItemStats>().itemStats.SquishAllModifierValues(Item2.GetComponent<ItemStats>().itemStats);


                        Destroy(craftingManager.Output_Slot.transform.GetChild(0).gameObject);
                    }
                }
            }
        }
    }

    public void LeftJoystickPlacement() {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        //Debug.Log(x + "] [" + y);

        if ((x >= 0.05f || x <= -0.05f) || (y >= 0.05f || y <= -0.05f)) {
            int oldOnSlot = OnSlot;
            if (OnSlot < 0) { OnSlot = 0; }
            else if (x > 00.3f && y < 0.4f && y > -0.4f) { OnSlot = 1; }
            else if (x < -0.3f && y < 0.4f && y > -0.4f) { OnSlot = 2; }
            else if (x < -0.1f && y > 00.3f) { OnSlot = 3; }
            else if (x > 00.1f && y > 00.3f) { OnSlot = 4; }
            else if (x < -0.1f && y < -0.3f) { OnSlot = 5; }
            else if (x > 00.1f && y < -0.3f) { OnSlot = 6; }
            else { return; }
            //Debug.Log(OnSlot);

            color = UnityEngine.Color.white;
            color.a = 0.2f;
                 if (oldOnSlot == 1) { craftingManager.Output_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 2) { craftingManager.Modifier_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 3) { craftingManager.Combined_First_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 4) { craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 5) { craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color; }
            else if (oldOnSlot == 6) { craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color; }

            color = UnityEngine.Color.red;
            color.a = 0.2f;
                 if (OnSlot == 1) { craftingManager.Output_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 2) { craftingManager.Modifier_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 3) { craftingManager.Combined_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 4) { craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 5) { craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 6) { craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color; }
        }

        if (Input.GetButton("CY") && timer <= 0.0f) {
            timer = 0.2f;

            bool InventoryHasRoom = false;
            if (menuManager.Inventory_Slot.GetComponent<Drop_Inventory>().NumberOfSlotsFilled < menuManager.Inventory_Slot.GetComponent<Drop_Inventory>().NumberOfSlotsTotal) { InventoryHasRoom = true; }
            int currentInventorySlot = menuManager.CurrentSlot;

            //pull
            //swap
            //push


            if (OnSlot == 1) { //Output_Slot
                if (craftingManager.Output_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Output_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Output_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Output_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Output_Slot.transform);
                }
                else if (craftingManager.Output_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Output_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 2) { //Modifier_Slot
                if (craftingManager.Modifier_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Modifier_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Modifier_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Modifier_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Modifier_Slot.transform);
                }
                else if (craftingManager.Modifier_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Modifier_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 3) { //Combined_First_Slot
                if (craftingManager.Combined_First_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Combined_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Combined_First_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Combined_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_First_Slot.transform);
                }
                else if (craftingManager.Combined_First_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_First_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 4) { //Combined_Second_Slot
                if (craftingManager.Combined_Second_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Combined_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Combined_Second_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Combined_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_Second_Slot.transform);
                }
                else if (craftingManager.Combined_Second_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Combined_Second_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 5) { //Disassemble_First_Slot
                if (craftingManager.Disassemble_First_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Disassemble_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Disassemble_First_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Disassemble_First_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_First_Slot.transform);
                }
                else if (craftingManager.Disassemble_First_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_First_Slot.transform); currentInventorySlot -= 1; }
            }
            else if (OnSlot == 6) { //Disassemble_Second_Slot
                if (craftingManager.Disassemble_Second_Slot.transform.childCount != 0 && currentInventorySlot == -1 && InventoryHasRoom) { craftingManager.Disassemble_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform); }
                else if (craftingManager.Disassemble_Second_Slot.transform.childCount != 0 && currentInventorySlot != -1) {
                    craftingManager.Disassemble_Second_Slot.transform.GetChild(0).SetParent(menuManager.Inventory_Slot.transform);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_Second_Slot.transform);
                }
                else if (craftingManager.Disassemble_Second_Slot.transform.childCount == 0 && currentInventorySlot != -1) { menuManager.Inventory_Slot.transform.GetChild(currentInventorySlot).SetParent(craftingManager.Disassemble_Second_Slot.transform); currentInventorySlot -= 1; }
            }

            menuManager.CurrentSlot = currentInventorySlot;
        }
    }

    public void DpadPlacement() {

        float x = Input.GetAxis("D-pad X");
        float y = Input.GetAxis("D-pad Y");

        if (x != 0.0f || y != 0) {
            int oldOnSlot = OnSlot;
            if (OnSlot < 0) { OnSlot = 0; }
            else if (x > 00.35f && y < 0.1f && y > -0.1f) { OnSlot = 1; }
            else if (x < -0.35f && y < 0.1f && y > -0.1f) { OnSlot = 2; }
            else if (x > -0.3f && x < -0.1f && y < 00.3f && y > 00.1f) { OnSlot = 3; }
            else if (x < 00.3f && x > 00.1f && y < 00.3f && y > 00.1f) { OnSlot = 4; }
            else if (x > -0.3f && x < -0.1f && y > -0.3f && y < -0.1f) { OnSlot = 5; }
            else if (x < 00.3f && x > 00.1f && y > -0.3f && y < -0.1f) { OnSlot = 6; }
            else { return; }
            Debug.Log(OnSlot);

            color = UnityEngine.Color.white;
            color.a = 0.2f;
                 if ( oldOnSlot == 1 ) {craftingManager.Output_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 2 ) {craftingManager.Modifier_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 3 ) {craftingManager.Combined_First_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 4 ) {craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 5 ) {craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color;}
            else if ( oldOnSlot == 6 ) {craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color;}

            color = UnityEngine.Color.red;
            color.a = 0.2f;
                 if (OnSlot == 1) { craftingManager.Output_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 2) { craftingManager.Modifier_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 3) { craftingManager.Combined_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 4) { craftingManager.Combined_Second_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 5) { craftingManager.Disassemble_First_Slot.GetComponent<Image>().color = color; }
            else if (OnSlot == 6) { craftingManager.Disassemble_Second_Slot.GetComponent<Image>().color = color; }
        }
    }
}