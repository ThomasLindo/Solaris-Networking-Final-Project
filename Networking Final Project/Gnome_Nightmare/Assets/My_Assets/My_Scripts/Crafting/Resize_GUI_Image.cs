using UnityEngine;

public class Resize_GUI_Image : MonoBehaviour {

    private bool ResizedTop = true;
    //Resize Top of GUI without effecting the other sides
    public void ResizeTop() {
        if (ResizedTop) { ResizedTop = false; }
        else { ResizedTop = true; }

        int AmountY = 100;
        if (ResizedTop) {this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y-AmountY); }
        else { this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y+AmountY);}
        
        if (ResizedTop) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y-AmountY/2, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y+AmountY/2, this.GetComponent<RectTransform>().localPosition.z); }
    }

    private bool ResizedBottom = true;
    //Resize Bottom of GUI without effecting the other sides
    public void ResizeBottom() {
        if (ResizedBottom) { ResizedBottom = false; }
        else { ResizedBottom = true; }

        int AmountY = 100;
        if (ResizedBottom) {this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y-AmountY); }
        else { this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y+AmountY);}
        
        if (ResizedBottom) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y+AmountY/2, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y-AmountY/2, this.GetComponent<RectTransform>().localPosition.z); }
    }




    //Resize GUI by an amount
    public void Resize(Vector4 amount) {

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x + amount.x, this.GetComponent<RectTransform>().sizeDelta.y + amount.y);

        if (amount.w == 0.0f) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y+amount.y/2, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y-amount.y/2, this.GetComponent<RectTransform>().localPosition.z); }

        if (amount.z == 0.0f) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x+amount.x/2, this.GetComponent<RectTransform>().localPosition.y, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x-amount.x/2, this.GetComponent<RectTransform>().localPosition.y, this.GetComponent<RectTransform>().localPosition.z); }
    }

    //Resize GUI by an amount (X,Y) to the Top, Bottom, Left, or Right
    public void Resize(int AmountX, int AmountY, bool TopDown, bool LeftRight) {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x + AmountX, this.GetComponent<RectTransform>().sizeDelta.y + AmountY);

        if (TopDown) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y+AmountY/2, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y-AmountY/2, this.GetComponent<RectTransform>().localPosition.z); }

        if (LeftRight) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x+AmountX/2, this.GetComponent<RectTransform>().localPosition.y, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x-AmountX/2, this.GetComponent<RectTransform>().localPosition.y, this.GetComponent<RectTransform>().localPosition.z); }
    }

    //Resize GUI by an amount (X) to the Left, or Right
    public void ResizeX(int AmountX, bool LeftRight) {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x + AmountX, this.GetComponent<RectTransform>().sizeDelta.y);

        if (LeftRight) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x+AmountX/2, this.GetComponent<RectTransform>().localPosition.y, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x-AmountX/2, this.GetComponent<RectTransform>().localPosition.y, this.GetComponent<RectTransform>().localPosition.z); }
    }
    //Resize GUI by an amount (Y) to the Top, or Bottom
    public void ResizeY(int AmountY, bool TopDown) {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y + AmountY);

        if (TopDown) { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y + AmountY / 2, this.GetComponent<RectTransform>().localPosition.z); }
        else { this.GetComponent<RectTransform>().localPosition = new Vector3(this.GetComponent<RectTransform>().localPosition.x, this.GetComponent<RectTransform>().localPosition.y - AmountY / 2, this.GetComponent<RectTransform>().localPosition.z); }
        
    }
}
