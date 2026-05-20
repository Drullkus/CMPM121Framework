using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectButton : Button {

	private TextMeshProUGUI _label;

	public void Initialize(Vector2 position, Level level) {
		_label.text = level.Name;

		transform.localPosition = (Vector3)position;

		onClick.AddListener(() => {
			EventBus.Instance.ChooseDifficulty(level);
		});
	}

}

public class DifficultySelect : MonoBehaviour {
	
	private List<Level> _levels;

	[SerializeField]
	private DifficultySelectButton _buttonPrefab;

	private void Awake() {
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
			Instantiate(_buttonPrefab).Initialize(new(130.0f - i * 40.0f, 0.0f),_levels[i]);
		}

		gameObject.SetActive(true);
	}

}
