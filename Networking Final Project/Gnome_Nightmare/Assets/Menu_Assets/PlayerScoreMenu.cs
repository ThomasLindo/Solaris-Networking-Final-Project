using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreMenu : MonoBehaviour {

    public GameObject StatsPanelPrefab;
    public GameObject ContentBox;

    private List<GameObject> PlayerPanel = new List<GameObject>();
    public GameObject[] Players;
    private float NextTimeToUpdate = 0.0f;

    private void Start() {
        LoadScorePanels();
    }

    private void Update() {
        if (Time.time >= NextTimeToUpdate){
            NextTimeToUpdate = Time.time + (1.0f);
            //OrderByKills();
            SetScorePanels();
        }
    }

    public void LoadScorePanels() {
        PlayerPanel.Clear();
        foreach (Transform child in ContentBox.transform) { Destroy(child.gameObject); }

        Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in Players) {
            GameObject temp = Instantiate<GameObject>(StatsPanelPrefab);
            temp.transform.Find("Player Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetPlayerName();
            //temp.transform.Find("Score Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetScore().ToString();
            temp.transform.Find("Kills Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetKills().ToString();
            temp.transform.Find("Downs Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetDowns().ToString();
            temp.transform.Find("Points Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetPoints().ToString();
            temp.transform.SetParent(ContentBox.transform);
            temp.transform.localScale = StatsPanelPrefab.transform.localScale;
            PlayerPanel.Add(temp);
        }
    }

    public void SetScorePanels() {
        foreach (GameObject playerPanel in PlayerPanel) {
            foreach (GameObject player in Players) {
                if (playerPanel.transform.Find("Player Text").GetComponent<Text>().text == player.GetComponent<PlayerStats>().GetPlayerName()) {
                    //playerPanel.transform.Find("Score Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetScore().ToString();
                    playerPanel.transform.Find("Kills Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetKills().ToString();
                    playerPanel.transform.Find("Downs Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetDowns().ToString();
                    playerPanel.transform.Find("Points Text").GetComponent<Text>().text = player.GetComponent<PlayerStats>().GetPoints().ToString();
                    //Debug.Log("Setting score");
                }
            }
        }
    }

    public void OrderByKills() {
        List<int> arrayOfNumbers = new List<int>();

        foreach (GameObject playerPanel in PlayerPanel) {
            arrayOfNumbers.Add(System.Convert.ToInt32(playerPanel.transform.Find("Kills Text").GetComponent<Text>().text));
        }
        arrayOfNumbers.Sort();
        arrayOfNumbers.Reverse();

        int i = 0;
        foreach (int num in arrayOfNumbers) {
            for (int j = i; j < ContentBox.transform.childCount; j++) {
                if (num == System.Convert.ToInt32(ContentBox.transform.GetChild(j).transform.Find("Kills Text").GetComponent<Text>().text)) {
                    ContentBox.transform.GetChild(j).SetSiblingIndex(i);
                    break;
                }
            }
            i++;
        }
    }

    public void OrderByDowns() {
        List<int> arrayOfNumbers = new List<int>();

        foreach (GameObject playerPanel in PlayerPanel) {
            arrayOfNumbers.Add(System.Convert.ToInt32(playerPanel.transform.Find("Downs Text").GetComponent<Text>().text));
        }
        arrayOfNumbers.Sort();
        arrayOfNumbers.Reverse();

        int i = 0;
        foreach (int num in arrayOfNumbers) {
            for (int j = i; j < ContentBox.transform.childCount; j++) {
                if (num == System.Convert.ToInt32(ContentBox.transform.GetChild(j).transform.Find("Downs Text").GetComponent<Text>().text)) {
                    ContentBox.transform.GetChild(j).SetSiblingIndex(i);
                    break;
                }
            }
            i++;
        }
    }

    public void OrderByPoints() {
        List<int> arrayOfNumbers = new List<int>();

        foreach (GameObject playerPanel in PlayerPanel) {
            arrayOfNumbers.Add(System.Convert.ToInt32(playerPanel.transform.Find("Points Text").GetComponent<Text>().text));
        }
        arrayOfNumbers.Sort();
        arrayOfNumbers.Reverse();

        int i = 0;
        foreach (int num in arrayOfNumbers) {
            for (int j = i; j < ContentBox.transform.childCount; j++) {
                if (num == System.Convert.ToInt32(ContentBox.transform.GetChild(j).transform.Find("Points Text").GetComponent<Text>().text)) {
                    ContentBox.transform.GetChild(j).SetSiblingIndex(i);
                    break;
                }
            }
            i++;
        }
    }
}
