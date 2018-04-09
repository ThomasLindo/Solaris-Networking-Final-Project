using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour {
    
    public void SliderToText(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = this.GetComponent<Slider>().value.ToString();
    }
    public void SliderTimeLimit(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " MIN");
        if (GameObject.Find("GameTimer")) { GameObject.Find("GameTimer").GetComponent<GameTimer>().SetTimeLimit(this.GetComponent<Slider>().value); }
    }
    public void SliderScoreLimit(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " Points");
    }
    public void SliderVolumeLimit(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = (((int)((this.GetComponent<Slider>().value + 80.0f) * 1.25f)).ToString() + " Vol");
    }
}
