using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DifficultySelect : MonoBehaviour {
	
	private List<Level> _levels;

	private Dictionary<string, PlayerClassData> _classes;

	[SerializeField]
	private DifficultySelectButton _difficultyButtonPrefab;

	[SerializeField]
	private GameObject _classButtonPrefab;

	private void Start() {
		AssetManager.Instance.LoadJson("levels", (loadedJson) => {
			 _levels = JsonConvert.DeserializeObject<List<Level>>(loadedJson);
		});

		AssetManager.Instance.LoadJson("classes", (loadedJson) => {
			_classes = JsonConvert.DeserializeObject<Dictionary<string, PlayerClassData>>(loadedJson);
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
			Instantiate(_difficultyButtonPrefab, transform).Initialize(new(0.0f, 130.0f - i * 40.0f), _levels[i]);
		}

		for(int i = 0; i < _classes.Count; i++) {
			string className = _classes.ElementAt(i).Key;
			PlayerClassData classData = _classes.ElementAt(i).Value;

			GameObject classSelectorButton = Instantiate(_classButtonPrefab, transform);
			classSelectorButton.transform.localPosition = new Vector3((i - (_classes.Count - 1) * 0.5f) * 175.0f, -200.0f, -100.0f);
			classSelectorButton.GetComponent<UI.ClassSelectorControl>().SetPlayerClass(className, classData, () => {
				EventBus.Instance.InvokeClassChosen(className, classData);
			});
		}

		gameObject.SetActive(true);
	}

}
