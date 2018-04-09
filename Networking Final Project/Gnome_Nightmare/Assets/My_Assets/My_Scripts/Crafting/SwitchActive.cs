using UnityEngine;

public class SwitchActive : MonoBehaviour {
    public bool isActive = false;
    //Will switch active state
    public void Switch() {
        if (this.gameObject.activeSelf) { this.gameObject.SetActive(false); isActive = false; }
        else { this.gameObject.SetActive(true); isActive = true; }
    }
}
