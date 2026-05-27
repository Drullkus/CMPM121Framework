using UnityEngine;

public class HealthBar : MonoBehaviour {

    public GameObject slider;
    
    public void SetHealth(float percent) {
        slider.transform.localScale = new Vector3(percent, 1, 1);
        slider.transform.localPosition = new Vector3(-(1-percent)/2, 0, 0);
    }

}
