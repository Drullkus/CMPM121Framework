using UnityEngine;

public class Reward : MonoBehaviour {

	private void Awake() {
		UIObject uiObject = new(
			UIState.REWARD,
			() => { gameObject.SetActive(true); },
			() => { gameObject.SetActive(false); }
		);

		EventBus.Instance.RegisterUIObject(uiObject);
	}

}
