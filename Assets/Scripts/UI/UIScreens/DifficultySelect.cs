using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelect : MonoBehaviour {
	
	private List<Level> _levels;

	[SerializeField]
	private DifficultySelectButton _buttonPrefab;

	private void Start() {
		AssetManager.Instance.LoadJson("levels", (loadedJson) => {
			 _levels = JsonConvert.DeserializeObject<List<Level>>(loadedJson);
		});

		UIScreen uiScreen = new(
			UIState.LEVEL_SELECT,
			Show,
			() => { gameObject.SetActive(false); }
		);

		EventBus.Instance.RegisterUIScreen(uiScreen);
	}

	private void Show() {
		for(int i = 0; i < _levels.Count; i++) {
			Instantiate(_buttonPrefab, transform).Initialize(new(0.0f, 130.0f - i * 40.0f), _levels[i]);
		}

		gameObject.SetActive(true);
	}

}
