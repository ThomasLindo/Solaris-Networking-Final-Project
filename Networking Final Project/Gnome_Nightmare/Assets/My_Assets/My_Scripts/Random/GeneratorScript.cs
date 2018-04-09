using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {
	public Text bPrompt;
	public GameObject smokeParticles;
	public bool isNew;
	public static bool isActive = true;     //	false means that the generator is broken
	public static bool eventIsActive = false;
	public static float timer = 10.0f;
	private GameObject smokeSpawn;
	private bool smokeSpawned = false;
	private bool promptSpawned = false;

	// Update is called once per frame
	void Update () {
		if (!isNew && isActive)
		{
			if (smokeSpawned)
			{
				Destroy(smokeSpawn);
			}
			//	play generator sound
		}
		else if(!isNew && !isActive)
		{
			if (!smokeSpawned)
			{
				smokeSpawn = Instantiate(smokeParticles, transform);
				smokeSpawned = true;
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == "Player" && eventIsActive)
		{
			if (!isNew && Event_Manager.instance.carrying)
			{
				Event_Manager.instance.newGennyPlaced = true;
			}
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject.name == "Player" && eventIsActive)
		{
			if (!isNew && Event_Manager.isEventActive(2, 2))
			{
				if (timer <= 10.0f && timer > 0.0f)
				{
					timer -= Time.deltaTime;
					bPrompt.text = "Repair Time: " + timer.ToString()[0] + timer.ToString()[1] + timer.ToString()[2] + timer.ToString()[3] + "s";
				}
				else if (timer <= 0.0f)
				{			
					//Destroy(smokeSpawn);
					//smokeSpawned = false;
					timer = 0.0f;
					bPrompt.text = "";
					Event_Manager.instance.gennyReplaced = true;
				}
			}
			else
			{
				if (!promptSpawned && !Event_Manager.instance.carrying && isNew)
				{
					bPrompt.text = "Press 'E' to Carry";
					promptSpawned = true;
					ButtonPrompt.promptActive = false;
				}

				if (Input.GetKeyDown(KeyCode.E))
				{
					bPrompt.text = "";
					promptSpawned = false;
					ButtonPrompt.promptActive = false;
					Event_Manager.instance.carrying = true;
				}
			}
		}
	}
}
