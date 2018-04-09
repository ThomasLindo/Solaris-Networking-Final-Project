using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    private bool IsOn = false;
    private float TimeLimit = 300.0f; //5.0f*60.0f=300.0f //60.0f*60.0f=3600.0f
    private float TimeSinceStarted = 0.0f;
    private float TimeLeft = 0.0f;

    private Text Timer_Text;

    private void Start() {
        DontDestroyOnLoad(this.transform.gameObject);
    }

    // Update is called once per frame
    private void Update () {
        if (IsOn && TimeSinceStarted == 0.0f) { FindUIText(); }

        if (IsOn && TimeSinceStarted < TimeLimit) {
            TimeSinceStarted += Time.deltaTime;
            TimeSinceStarted = Mathf.Clamp(TimeSinceStarted, 0.0f, TimeLimit);
            TimeLeft = TimeLimit - TimeSinceStarted;
            TimeLeft = Mathf.Clamp(TimeLeft, 0.0f, TimeLimit);
            //Debug.Log(GetTimeSinceStarted() + "/" + GetTimeLimit());
            UpdateUI();
        }
        else if (IsOn && TimeSinceStarted >= TimeLimit) {
            //Debug.Log("End Game");
        }
    }

    public void TurnOn() { IsOn = true; }
    public void TurnOff() { IsOn = false; }

    public void SetTimeLimit(float timeLimit) { TimeLimit = timeLimit * 60.0f; }
    public float GetTimeLimit() { return TimeLimit; }
    public float GetTimeSinceStarted() { return TimeSinceStarted; }
    public float GetTimeLeft() { return TimeLeft; }

    public void FindUIText() {
        if (GameObject.Find("Timer_Text")) { Timer_Text = GameObject.Find("Timer_Text").GetComponent<Text>(); }
    }
    public void UpdateUI() {
        if (Timer_Text == null) { return; }
        Timer_Text.text = ("[" + (int)GetTimeSinceStarted() + "/" + GetTimeLimit() + "]");
    }

}
