using UnityEngine;
using UnityEngine.UI;

public class ToggleMultipleButtons : MonoBehaviour {

    public void SwitchThis() {
        if (this.gameObject.GetComponent<Toggle>().isOn) { this.gameObject.GetComponent<Toggle>().isOn = false; }
        //else { this.gameObject.GetComponent<Toggle>().isOn = true; }

    }
}
