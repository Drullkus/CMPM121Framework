using UnityEngine;

public class ManaBar : MonoBehaviour {
	
	[SerializeField]
	private GameObject _slider;

	public void SetMana(float ratio) {
		_slider.transform.localScale = new Vector3(ratio, 1.0f, 1.0f);
		_slider.transform.localPosition = new Vector3((ratio - 1.0f) / 2.0f, 0.0f, 0.0f);
	}

}
