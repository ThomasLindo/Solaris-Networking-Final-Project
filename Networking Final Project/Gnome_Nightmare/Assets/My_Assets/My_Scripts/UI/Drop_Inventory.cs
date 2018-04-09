using UnityEngine;
using UnityEngine.EventSystems; //used for drag and drop

public class Drop_Inventory : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public int NumberOfSlotsTotal = 1;
    public int NumberOfSlotsFilled = 0;
    public Drag_Inventory.Slot typeOfItem = Drag_Inventory.Slot.Inventory;


    void FixedUpdate() {
        NumberOfSlotsFilled = this.transform.childCount;
        if (this.transform.Find("placeholder") == true) { NumberOfSlotsFilled--; }
    }

    void IDropHandler.OnDrop(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        if (typeOfItem == Drag_Inventory.Slot.Internet) { return; }
        Drag_Inventory DI = eventData.pointerDrag.GetComponent<Drag_Inventory>();
        if (DI != null && NumberOfSlotsFilled <= NumberOfSlotsTotal) {
            if ((typeOfItem == DI.typeOfItem || typeOfItem == Drag_Inventory.Slot.Inventory) && (NumberOfSlotsFilled < NumberOfSlotsTotal)) {
                DI.parentToReturnTo = this.transform;
            }
            if (typeOfItem == Drag_Inventory.Slot.Drop_To_Floor) {
                DI.parentToReturnTo = null;
                GameObject Item = (GameObject)Instantiate(DI.ItemOnDrop);
                Item.transform.SetParent(GameObject.FindWithTag("Items_Spawn_Here").transform);
                Item.name = DI.ItemOnDrop.name;// + " Not Prefab";
                Vector3 temp = GameObject.FindWithTag("Player").transform.position;
                Item.transform.position = new Vector3(temp.x, temp.y - 0.50f, temp.z);

                if (Item.transform.GetChild(0).GetComponent<Gun_Behaviour>() && DI.gameObject.GetComponent<ItemStats>()) {
                    Debug.Log("Drop");
                    //Debug.Log("Drop : Before " + Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats[4].baseValue + " After " + DI.gameObject.GetComponent<ItemStats>().itemStats[4].baseValue);
                    Item.transform.GetChild(0).GetComponent<Gun_Behaviour>().Stats = DI.gameObject.GetComponent<ItemStats>().itemStats;
                }
                if (Item.GetComponent<Ammo_Types>() && DI.gameObject.GetComponent<Ammo_Types>()) {
                    Item.GetComponent<Ammo_Types>().Amount = DI.gameObject.GetComponent<Ammo_Types>().Amount;
                }

                //Instantiate the Agent so that it will send to the other clients
                Agent tempAgent;
                if (Item.GetComponent<Agent>()) { tempAgent = Item.GetComponent<Agent>(); }
                else {
                    Item.AddComponent<Agent>();
                    tempAgent = Item.GetComponent<Agent>();
                }
                tempAgent.AgentNumber = ID_Table.instance.ItemList[0];
                ID_Table.instance.ItemList.RemoveAt(0);
                tempAgent.RepeatEvery = 10.0f;
                tempAgent.SendInstantiate(Item.transform.position);


                //Drop Item to floor
                Destroy(DI.gameObject);
                Destroy(DI.placeholder.gameObject);
            }
        }
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        if (typeOfItem == Drag_Inventory.Slot.Internet) { return; }
        Drag_Inventory DI = eventData.pointerDrag.GetComponent<Drag_Inventory>();
        if (DI != null) {
            if ((typeOfItem == DI.typeOfItem || typeOfItem == Drag_Inventory.Slot.Inventory) && (NumberOfSlotsFilled < NumberOfSlotsTotal)) {
                DI.placeholderParent = this.transform;
            }
        }
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        if (typeOfItem == Drag_Inventory.Slot.Internet) { return; }
        Drag_Inventory DI = eventData.pointerDrag.GetComponent<Drag_Inventory>();
        if (DI != null && DI.placeholderParent == this.transform) {
            if ((typeOfItem == DI.typeOfItem || typeOfItem == Drag_Inventory.Slot.Inventory) && (NumberOfSlotsFilled < NumberOfSlotsTotal)) {
                DI.placeholderParent = DI.parentToReturnTo;
            }
        }
    }
}
