using UnityEngine;

public class SwitchOn : MonoBehaviour {
    public GameObject SwitchThis;
    public bool IsActive = false;

    public bool IsOn() {
        if (SwitchThis.activeSelf) { return true; }
        else { return false; }
    }
	public void Switch() {
        if (SwitchThis.activeSelf) { SwitchThis.SetActive(false); }
        else { SwitchThis.SetActive(true); }
    }
    public void SwitchON() { SwitchThis.SetActive(true); }
    public void SwitchOFF() { SwitchThis.SetActive(false); }
    
}
