using UnityEngine;

public class Ammo_Inventory : MonoBehaviour {

    private float timer = 0.0f;
    // Update is called once per frame
    void Update () {
        if (timer > 0.0f) { timer -= Time.deltaTime; }
        else if (timer <= 0.0f) {
            timer = 0.25f;
            UpdateAmount();
            //CombindStacks();
        }
    }
    public bool CombindStacks(){
        if (this.transform.childCount < 2) { return false; }
       
        for (int i = 0; i < this.transform.childCount; i++) {
            for (int j = 0; j < this.transform.childCount; j++) {
                if (i != j) {
                    if (this.transform.GetChild(i).GetComponent<Ammo_Types>() && this.transform.GetChild(j).GetComponent<Ammo_Types>()) {
                        if (this.transform.GetChild(i).GetComponent<Ammo_Types>().TypeOfAmmo == this.transform.GetChild(j).GetComponent<Ammo_Types>().TypeOfAmmo) {
                            this.transform.GetChild(i).GetComponent<Ammo_Types>().Amount += this.transform.GetChild(j).GetComponent<Ammo_Types>().Amount;
                            Destroy(this.transform.GetChild(j).gameObject);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    private void UpdateAmount() {
        for (int i = 0; i < this.transform.childCount; i++) {
            if (this.transform.GetChild(i).GetComponent<Ammo_Types>()) {
                this.transform.GetChild(i).GetComponent<Ammo_Types>().SetTextToAmount();
            }
        }
    }
    
    public int CheckAmount(Ammo_Types.Ammo type) {
        for (int i = 0; i < this.transform.childCount; i++) {
            if (this.transform.GetChild(i).GetComponent<Ammo_Types>().TypeOfAmmo == type) {
                return this.transform.GetChild(i).GetComponent<Ammo_Types>().Amount;
            }
        }
        return -1;
    }
}
