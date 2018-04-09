using UnityEngine;
using UnityEngine.EventSystems; //used for drag and drop

public class Drag_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private Vector2 MouseDifference;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        MouseDifference = eventData.position - new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
        //MouseDifference = eventData.position - new Vector2(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y);
    }
    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        this.transform.localPosition = eventData.position - MouseDifference;
        //this.GetComponent<RectTransform>().localPosition = eventData.position - MouseDifference;
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
    }

}
