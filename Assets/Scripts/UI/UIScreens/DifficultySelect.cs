using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultySelect : MonoBehaviour {
	
	private List<Level> _levels;

	[SerializeField]
	private List<TextMeshProUGUI> _levelLabels;

	private void Awake() {
		AssetManager.Instance.LoadJson("levels", (loadedJson) => {
			 _levels = JsonConvert.DeserializeObject<List<Level>>(loadedJson);
		});

		UIScreen uiScreen = new(
			UIState.LEVEL_SELECT,
			Show,
			() => { gameObject.SetActive(false); }
		);

		EventBus.Instance.RegisterUIObject(uiScreen);
	}

	private void Show() {
		gameObject.SetActive(true);
	}

}
