using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HellRoundUIColorChange : MonoBehaviour {
	public static bool doCheck = false;

	// Update is called once per frame
	void Update () {
		if (doCheck)
		{
			if (EnemySpawners.Interface_SpawnTable.instance.CurrentLevel % 6 == 0)
			{
				transform.GetComponent<Text>().color = Color.red;
			}
			else
			{
				transform.GetComponent<Text>().color = new Color32(190, 190, 190, 255);
			}
			doCheck = false;
		}
	}
}
