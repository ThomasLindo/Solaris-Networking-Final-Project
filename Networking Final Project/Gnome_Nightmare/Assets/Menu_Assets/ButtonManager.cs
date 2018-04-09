using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class ButtonManager : SerializedMonoBehaviour {

    public static ButtonManager instance;
	private bool isLonely = true;

	void Awake() {
		instance = this; HideAll();
		if (Client_Manager.instance || Server_Manager.instance) { isLonely = false; }
	}

    [ToggleGroup("Settings_Menu", order: 0, groupTitle: "Settings Menu")]
    public bool Settings_Menu;
    [ToggleGroup("Settings_Menu")]
    public GameObject LoadMenu;
    [ToggleGroup("Settings_Menu")]
    public GameObject MultiplayerMenu;
    [ToggleGroup("Settings_Menu")]
    public GameObject SettingsMenu;
    [ToggleGroup("Settings_Menu")]
    public GameObject CreditMenu;


    [ToggleGroup("Pause_Menu", order: 1, groupTitle: "Pause Menu")]
    public bool Pause_Menu;
    [ToggleGroup("Pause_Menu")]
    public GameObject PauseMenu;

    public void OpenPauseMenu() {
		PauseMenu.GetComponent<UnHide>().View();
		Cursor.lockState = CursorLockMode.None;
		if (!isLonely) { return; }
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
    public void ClosePauseMenu() {
		HideAll();
		PlayerManager.instance.MenuOpen = false;
		Cursor.lockState = CursorLockMode.Locked;
		if (!isLonely) { return; }
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}


    [ToggleGroup("Death_Menu", order: 2, groupTitle: "Death Menu")]
    public bool Death_Menu;
    [ToggleGroup("Death_Menu")]
    public GameObject DeathMenu;

    public void OpenDeathMenu() {
        DeathMenu.GetComponent<UnHide>().View();
        PlayerManager.instance.MenuOpen = false;
        Cursor.lockState = CursorLockMode.None;
    }
    public void CloseDeathMenu() {
        HideAll();
        PlayerManager.instance.MenuOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    [ToggleGroup("Score_Menu", order: 3, groupTitle: "Score Menu")]
    public bool Score_Menu;
    [ToggleGroup("Score_Menu")]
    public GameObject ScoreMenu;

    public void OpenScoreMenu() {
        ScoreMenu.GetComponent<UnHide>().View();
        Cursor.lockState = CursorLockMode.None;
    }
    public void CloseScoreMenu() {
        HideAll();
        PlayerManager.instance.MenuOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    //public void SinglePlayerButton(string newGame) { /*SceneManager.LoadScene(newGame);*/ }
    public void SinglePlayerButton() {
        SettingsMenu.GetComponent<UnHide>().Hide();
        CreditMenu.GetComponent<UnHide>().Hide();
        MultiplayerMenu.GetComponent<UnHide>().Hide();
        LoadMenu.GetComponent<UnHide>().HideUnHide();
    }
    public void MultiPlayerButton() {
        SettingsMenu.GetComponent<UnHide>().Hide();
        CreditMenu.GetComponent<UnHide>().Hide();
        LoadMenu.GetComponent<UnHide>().Hide();
        MultiplayerMenu.GetComponent<UnHide>().HideUnHide();
    }
    public void SettingsButton() {
        LoadMenu.GetComponent<UnHide>().Hide();
        MultiplayerMenu.GetComponent<UnHide>().Hide();
        CreditMenu.GetComponent<UnHide>().Hide();
        SettingsMenu.GetComponent<UnHide>().HideUnHide();
    }
    public void CreditButton() {
        LoadMenu.GetComponent<UnHide>().Hide();
        MultiplayerMenu.GetComponent<UnHide>().Hide();
        SettingsMenu.GetComponent<UnHide>().Hide();
        CreditMenu.GetComponent<UnHide>().HideUnHide();
    }
    public void QuitButton() { HideAll(); Application.Quit(); }

    public void ResumeButton() {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        HideAll();
        PlayerManager.instance.MenuOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReviveButton() {
        PlayerManager.instance.player.GetComponent<PlayerStats>().FullHealth();
        PlayerManager.instance.player.GetComponent<PlayerStats>().isDead = false;
        CloseDeathMenu();
    }
    public void MainMenuButton() { SceneManager.LoadScene("Menu_Start"); }


    private void HideAll() {
        if (LoadMenu) { LoadMenu.GetComponent<UnHide>().Hide(); }
        if (MultiplayerMenu) { MultiplayerMenu.GetComponent<UnHide>().Hide(); }
        if (SettingsMenu) { SettingsMenu.GetComponent<UnHide>().Hide(); }
        if (CreditMenu) { CreditMenu.GetComponent<UnHide>().Hide(); }
        if (PauseMenu) { PauseMenu.GetComponent<UnHide>().Hide(); }
        if (ScoreMenu) { ScoreMenu.GetComponent<UnHide>().Hide(); }
        if (DeathMenu) { DeathMenu.GetComponent<UnHide>().Hide(); }
    }
}
