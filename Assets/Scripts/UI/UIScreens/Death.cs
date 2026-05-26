using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour {
	
	[SerializeField]
	private Button _restartButton;

	private void Start() {
		UIScreen uiScreen = new(
			UIState.DEATH,
			() => { gameObject.SetActive(true); },
			() => { gameObject.SetActive(false); }
		);

		_restartButton.onClick.AddListener(EventBus.Instance.RequestRestart);
		_restartButton.onClick.AddListener(EventBus.Instance.CloseUIScreen);

		EventBus.Instance.RegisterUIScreen(uiScreen);

		gameObject.SetActive(false);
	}

}
