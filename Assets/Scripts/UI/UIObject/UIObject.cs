using UnityEngine;

public class UIObject : MonoBehaviour {

	public UIState state = UIState.WAVE;

	public void Show() { gameObject.SetActive(true); }
	public void Hide() { gameObject.SetActive(false); }

}
