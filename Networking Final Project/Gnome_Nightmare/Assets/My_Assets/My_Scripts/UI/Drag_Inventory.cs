using UnityEngine.UI; //used for LayoutElement
using UnityEngine;
using UnityEngine.EventSystems; //used for drag and drop

public class Drag_Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    public enum Slot { Inventory, Miscellaneous, Weapon, Head, Chest, Legs, Drop_To_Floor, None, Ammo, Internet, Extinguisher, Healing };
    public Slot typeOfItem = Slot.Inventory;
    public Ammo_Types.Ammo typeOfAmmo = Ammo_Types.Ammo.Basic;

    [HideInInspector]
    public Transform parentToReturnTo = null;
    [HideInInspector]
    public Transform placeholderParent = null;
    [HideInInspector]
    public GameObject placeholder = null;
    public GameObject ItemOnDrop = null;

    private Vector2 MouseDifference;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        if (this.transform.parent.GetComponent<Drop_Inventory>() && this.transform.parent.GetComponent<Drop_Inventory>().typeOfItem == Slot.Internet) { return; }
        //Create a placeholder
        placeholder = new GameObject();
        placeholder.name = "placeholder";
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement placeholder_layoutElement = placeholder.AddComponent<LayoutElement>();
        placeholder_layoutElement.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        placeholder_layoutElement.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        placeholder_layoutElement.flexibleWidth = 0;
        placeholder_layoutElement.flexibleHeight = 0;
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        //Find where the user clicked on the Item
        MouseDifference = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);

        //set the parent(inventory) of the item
        parentToReturnTo = this.transform.parent;
        placeholderParent = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        //Allows the OnDrop function to work
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        if (this.transform.parent.GetComponent<Drop_Inventory>() && this.transform.parent.GetComponent<Drop_Inventory>().typeOfItem == Slot.Internet) { return; }

        //Item being dragged will follow the pointer
        this.transform.position = eventData.position - MouseDifference;

        if (placeholder.transform.parent != placeholderParent) {
            placeholder.transform.SetParent(placeholderParent);
        }

        //
        int newSiblingIndex = placeholderParent.childCount;
        for (int i = 0; i < placeholderParent.childCount; i++) {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x) {
                if (this.transform.position.y < placeholderParent.GetChild(i).position.y + 50.0f) {
                    if (this.transform.position.y > placeholderParent.GetChild(i).position.y - 50.0f) {
                        newSiblingIndex = i;
                        if (placeholder.transform.GetSiblingIndex() < newSiblingIndex) {
                            newSiblingIndex--;
                        }
                        break;
                    }
                }
                if (this.transform.position.x > placeholderParent.GetChild(i).position.x) {
                    //- (placeholderParent.GetChild(i).GetComponent<RectTransform>().rect.width/2.0f)
                    Debug.Log("BOOM");
                }
            }
        }
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        if (this.transform.parent.GetComponent<Drop_Inventory>() && this.transform.parent.GetComponent<Drop_Inventory>().typeOfItem == Slot.Internet) { return; }

        //Return to the correct place
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

        //Enable so that you can pick up the Item again
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //DISTROY the placeholder
        Destroy(placeholder);

        //Apply Status boosts from equipment
        //Weapons and stuff
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) { }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) { }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        Items_Manager.instance.SetCurrentlySelectedItem(this.transform.GetSiblingIndex());
        Items_Manager.instance.SetInfo(this.transform.GetSiblingIndex());
    }
    
}
