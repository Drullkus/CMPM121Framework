using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WaveLabelController : MonoBehaviour {

    private TextMeshProUGUI _label;
	private string _newText = "";

	private int _enemyCount = 0;

    private void Awake() {
        _label = GetComponent<TextMeshProUGUI>();

		EventBus.Instance.OnWaveStarted += (_, enemyCount) => {
			_enemyCount = enemyCount;
			_newText = $"Enemies remaining: {_enemyCount}";
		};

		EventBus.Instance.OnEnemyDefeated += () => {
			_newText = $"Enemies remaining: {--_enemyCount}";
		};

		EventBus.Instance.OnWaveEnded += () => {
			gameObject.SetActive(false);
		};
    }

	private void Update() {
		if(_newText == "") { return; }

		_label.SetText(_newText);
		_newText = "";
	}

}
