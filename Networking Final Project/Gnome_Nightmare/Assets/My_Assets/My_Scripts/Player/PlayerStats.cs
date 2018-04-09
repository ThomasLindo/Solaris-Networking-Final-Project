using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats {

    public string PlayerName = "Player";
    private int PlayerLevel = 1;
    private float PlayerExperience = 0;
    private int MaxExperienceForLevel = 10;
    private float Points = 50;
    private int Kills = 0;
    private int Downs = 0;

    private Transform HealthBarObj;
    private Text ScoreBox;

    public bool isDead = false;
    public float TotalHealth;

    private void Start() { }

    private void Update() {
        HealthBar();
    }

    //Downs
    public string GetPlayerName() { return PlayerName; }
    public void SetPlayerName(string name) { PlayerName = name; }

    //Points
    public float GetPoints() { return Points; }
    public void AddPoints(float amount) { Points += amount; }
    public void UsePoints(float amount) { Points -= amount; }
    public bool CheckPoints(float amount) {
        if (Points >= amount) { return true; }
        else { return false; }
    }

    //Kills
    public int GetKills() { return Kills; }
    public void AddKills(int amount) { Kills += amount; }

    //Downs
    public int GetDowns() { return Downs; }
    public void AddDowns(int amount) { Downs += amount; }

    //Adds Experience to the player
    //If the player Experience is full, level up
    public void AddExperience(float amount) {
        PlayerExperience += amount;
        if (PlayerExperience >= MaxExperienceForLevel) { PlayerLevelUp(); }
    }

    //When the player levels up
    private void PlayerLevelUp() {
        //Add player modifiers
        //Damage.AddModifier(0.50f);
        //Armour.AddModifier(0.50f);
        //FullHealth();

        //Incress player Level
        PlayerLevel += 1;
        PlayerExperience -= MaxExperienceForLevel;
        MaxExperienceForLevel = (PlayerLevel*PlayerLevel*4);

        //If the player can still level up, do so
        if (PlayerExperience >= MaxExperienceForLevel) { PlayerLevelUp(); }
        
    }

    private void HealthBar(){
        TotalHealth = CurrentHealth;
        if (CurrentHealth <= 0.0f && !isDead) {
            isDead = true;
            Downs++;
            return;
        }

        if (!HealthBarObj) { HealthBarObj = GameObject.Find("HealthBar").transform; }
        else {
            float v_Health = CurrentHealth / MaxHealth;
            v_Health = Mathf.Clamp(v_Health, 0.0f, 1.0f);
            Vector3 Health = new Vector3(v_Health, 1.0f, 1.0f);
            HealthBarObj.GetChild(1).GetComponent<RectTransform>().transform.localScale = Health;
            Health = new Vector3(1.0f - (v_Health), 1.0f, 1.0f);
            HealthBarObj.GetChild(2).GetComponent<RectTransform>().transform.localScale = Health;
        }
        if (ScoreBox == null) { if (GameObject.Find("Score box")) { ScoreBox = GameObject.Find("Score box").GetComponent<Text>(); } }
        else if (ScoreBox != null) { ScoreBox.text = "Money: " + Points.ToString(); }
        //if (this.gameObject.transform.Find("HealthBarTop")) {
        //    float v_Health = CurrentHealth / MaxHealth;
        //    v_Health = Mathf.Clamp(v_Health, 0.0f, 1.0f);
        //    Vector3 Health = new Vector3(v_Health, 1.0f, 1.0f);
        //    this.gameObject.transform.Find("HealthBarTop").GetChild(0).GetChild(0).GetComponent<RectTransform>().transform.localScale = Health;
        //    Health = new Vector3(1.0f-(v_Health), 1.0f, 1.0f);
        //    this.gameObject.transform.Find("HealthBarTop").GetChild(0).GetChild(1).GetComponent<RectTransform>().transform.localScale = Health;
        //}
    }
}
