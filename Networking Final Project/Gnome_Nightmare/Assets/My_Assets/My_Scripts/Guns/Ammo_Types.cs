using UnityEngine;
using UnityEngine.UI;

public class Ammo_Types : MonoBehaviour {

    public enum Ammo { None, Basic, Paintball, Staples, Arrow, Potato, Scrap, Fuel, Extinguisher };
    public Ammo TypeOfAmmo = Ammo.None;

    //[SerializeField]
    public int Amount;
    public GameObject AmountText;

    private void Start() {
        //if (AmountText != null && this.transform.Find("Amount_Text")) { AmountText = this.transform.Find("Amount_Text").gameObject; }
    }

    public void SetTextToAmount() {
        if (Amount <= 0) { Destroy(this.gameObject); }
        if (AmountText != null) { AmountText.GetComponent<Text>().text = Amount.ToString(); }
    }

}
