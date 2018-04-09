using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerScript : MonoBehaviour {
	public Text bPrompt;
	public UnHide lights;
	public UnHide computer;
    public UnHide GreenLight;
    public UnHide RedLight;
    public static bool isActive = true;
	private float timer = 0.0f;
	private bool lightsAreOff = false;

	private void Update() {
		if (!isActive && !lightsAreOff) {
			computer.Hide();
			lights.Hide();
			lightsAreOff = true;
            GreenLight.Hide();
            RedLight.View();
        }
		else if(isActive && lightsAreOff) {
			computer.View();
			lights.View();
			lightsAreOff = false;
            GreenLight.View();
            RedLight.Hide();
        }
	}

    

	private void OnCollisionEnter(Collision other) {
		if(other.gameObject.name == "Player") {
            if (Event_Manager.isEventActive(1, 2)) {
                if (!ButtonPrompt.promptActive) {
                    bPrompt.text = "Press 'E' To Turn On";
                    ButtonPrompt.promptActive = true;
                }
            }
            else if (!lightsAreOff) {
                if (!ButtonPrompt.promptActive) {
                    bPrompt.text = "This is used to turn the power to the farm on.";
                    ButtonPrompt.promptActive = true;
                }
            }
		}
	}

	private void OnCollisionStay(Collision other) {
		if (other.gameObject.name == "Player") {
			if (Event_Manager.isEventActive(1, 2)) {
				if (Input.GetKeyDown(KeyCode.E)) { isActive = true; }
				else if (Input.GetKeyUp(KeyCode.E)) { timer = 0.0f; }
			}
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.name == "Player") {
			if (ButtonPrompt.promptActive) {
				bPrompt.text = "";
				ButtonPrompt.promptActive = false;
			}
		}
	}

    private void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == "Player") {
            if (Event_Manager.isEventActive(1, 2)) {
                if (!ButtonPrompt.promptActive) {
                    bPrompt.text = "Press 'E' To Turn On";
                    ButtonPrompt.promptActive = true;
                }
            }
            else if (!lightsAreOff) {
                if (!ButtonPrompt.promptActive) {
                    bPrompt.text = "This is used to turn the power to the farm on.";
                    ButtonPrompt.promptActive = true;
                }
            }
		}
	}
    
	private void OnTriggerStay(Collider other) {
		if (other.gameObject.name == "Player") {
            Debug.Log("Bunker");
			if (Event_Manager.isEventActive(1, 2)) {
				if (Input.GetKeyDown(KeyCode.E)) { isActive = true; }
				else if (Input.GetKeyUp(KeyCode.E)) { timer = 0.0f; }
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.name == "Player") {
			if (ButtonPrompt.promptActive) {
				bPrompt.text = "";
				ButtonPrompt.promptActive = false;
			}
		}
	}
}
