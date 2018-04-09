using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour {

    public GameObject dropDown;
    public List<string> ExcludedIfContains;

    private void Awake() {
        if (dropDown) { LoadScenesToDropDown(dropDown); }
    }


    public void LoadSceneFromString(string newGame) { SceneManager.LoadScene(newGame); }

    public void LoadSceneFromDropDown(GameObject dropDown) {
        int SceneNum = dropDown.GetComponent<Dropdown>().value;
        //Debug.Log(dropDown.GetComponent<Dropdown>().options[SceneNum].text);
        SceneManager.LoadScene(dropDown.GetComponent<Dropdown>().options[SceneNum].text);
        GameObject.Find("GameTimer").GetComponent<GameTimer>().TurnOn();
    }

    public void LoadScenesToDropDown(GameObject dropDown) {
        dropDown.GetComponent<Dropdown>().ClearOptions();
        List<string> SceneNames = new List<string>();
        //Debug.Log(SceneManager.sceneCountInBuildSettings);
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++){
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);

            bool Exclude = false;
            for (int j = 0; j < ExcludedIfContains.Count; j++) {
                if (sceneName.Contains(ExcludedIfContains[j])) { Exclude = true; break; }
            }
            if (!Exclude) {
                bool AlreadyIncluded = false;
                for (int k = 0; k < SceneNames.Count; k++) {
                    if (SceneNames[k] == sceneName) { AlreadyIncluded = true; break; }
                }
                if (!AlreadyIncluded) { SceneNames.Add(sceneName); }
            }

            //if (ExcludedIfContains.Count == 0) { SceneNames.Add(sceneName); }

            //Debug.Log(sceneName);
        }
        dropDown.GetComponent<Dropdown>().AddOptions(SceneNames);
    }
}
