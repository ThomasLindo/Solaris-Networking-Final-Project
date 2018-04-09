using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap_Zoom : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private Vector2 MouseStart;
    private Vector2 MouseMove;
    private Vector2 MouseDifference;
    public Camera MiniMap_Camera;
    public Vector2 ClampDistance;

    private void Start() {
        Camera MiniMap = (Camera)Instantiate(MiniMap_Camera);
        MiniMap.name = "MiniMap_Camera";
        MiniMap.transform.SetParent(this.transform);
    }


    //When a player clicks the minimap, get position of the mouse
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        //Set starting position
        MouseStart = eventData.position;
    }

    //When a player drags the minimap, resize it
    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        //Set new mouse position
        MouseMove = eventData.position;
        //Set the difference between starting position and mouse position
        MouseDifference = MouseMove - MouseStart;
        float Temp = MouseDifference.magnitude * 0.1f;
        //Clamp the distance so player can't voom in and out infinity
        Temp = Mathf.Clamp(Temp, ClampDistance.x, ClampDistance.y);
        GameObject.Find("MiniMap_Camera").GetComponent<Camera>().orthographicSize = Temp;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        if (eventData.pointerDrag == null) { return; }
        //Set the difference between starting position and mouse position
        MouseDifference = MouseMove - MouseStart;
        float Temp = MouseDifference.magnitude * 0.1f;
        //Clamp the distance so player can't voom in and out infinity
        Temp = Mathf.Clamp(Temp, ClampDistance.x, ClampDistance.y);
        GameObject.Find("MiniMap_Camera").GetComponent<Camera>().orthographicSize = Temp;
    }
}
