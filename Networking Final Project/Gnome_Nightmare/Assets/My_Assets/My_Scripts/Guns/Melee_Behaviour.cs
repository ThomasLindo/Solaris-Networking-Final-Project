using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Melee_Behaviour : SerializedMonoBehaviour {

    [TableList]
    public OdinTables.WeaponStatsTable Stats = new OdinTables.WeaponStatsTable();

    public PlayerManager playerManager;
    protected MenuManager menuManager;
    protected Camera PlayerCamera;

    // Use this for initialization
    void Start () {
        Invoke("DelayedStart", 0.1f);
        PlayerCamera = Camera.main;
        playerManager = PlayerManager.instance;
        menuManager = MenuManager.instance;
    }

    //Used so that everything gets a chance to load before trying to accsess it
    private void DelayedStart() { }

    // Update is called once per frame
    void Update () {
        if (this.playerManager.MenuOpen) { return; }
        if (this.playerManager.player.GetComponent<PlayerStats>().isDead) { return; }
        MeleeWeapons_Update();
    }

    #region MeleeWeapons
    protected float SwingProgress = 0.0f;

    [BoxGroup("Weapon Data", true, true)]
    [HorizontalGroup("Weapon Data/Group 0", 0.33f), LabelWidth(90)]
    public bool weaponSwung = false;
    [HorizontalGroup("Weapon Data/Group 0", 0.33f), LabelWidth(90)]
    public bool DownSwing = true;
    //[HorizontalGroup("Weapon Data/Group 0", 0.33f), LabelWidth(90)]
    //public float SwingSpeed = 0.1f;
    [HorizontalGroup("Weapon Data/Group 1", 0.5f), LabelWidth(90)]
    public Vector3 StartRot = Vector3.zero;
    [HorizontalGroup("Weapon Data/Group 1", 0.5f), LabelWidth(90)]
    public Vector3 StartPos = Vector3.zero;
    [HorizontalGroup("Weapon Data/Group 2", 0.5f), LabelWidth(50)]
    public Vector3 EndRot = Vector3.zero;
    [HorizontalGroup("Weapon Data/Group 2", 0.5f), LabelWidth(50)]
    public Vector3 EndPos = Vector3.zero;
    [HorizontalGroup("Weapon Data/Group 3", 0.5f), LabelWidth(90)]
    public Vector3 CurrentRot = Vector3.zero;
    [HorizontalGroup("Weapon Data/Group 3", 0.5f), LabelWidth(90)]
    public Vector3 CurrentPos = Vector3.zero;

    void MeleeWeapons_Update() {
        if (weaponSwung == false && (Input.GetButton("Fire1") || Input.GetAxis("Right Trigger") != 0.0f)) {
            weaponSwung = true;
            SwingProgress = 0.0f;
        }
        else if (weaponSwung) {
            SwingProgress += (this.Stats.FireRate.GetValue() * Time.deltaTime);
            // Swing weapon
            if (DownSwing) { 
                CurrentPos = Vector3.Lerp(StartPos, EndPos, SwingProgress);
                CurrentRot = Vector3.Lerp(StartRot, EndRot, SwingProgress);
                if (CurrentRot == EndRot && CurrentPos == EndPos) { DownSwing = false; SwingProgress = 0.0f; }
            }
            else if (!DownSwing) {
                CurrentPos = Vector3.Lerp(EndPos, StartPos, SwingProgress);
                CurrentRot = Vector3.Lerp(EndRot, StartRot, SwingProgress);
                if (CurrentRot == StartRot && CurrentPos == StartPos) { DownSwing = true; weaponSwung = false; SwingProgress = 0.0f; }
            }
            this.transform.GetChild(0).localPosition = CurrentPos;
            this.transform.GetChild(0).localRotation = Quaternion.Euler(CurrentRot);
        }
    }
    
    
    #endregion
}
