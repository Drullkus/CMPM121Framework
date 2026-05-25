using System.Timers;
using TMPro;
using UnityEngine;

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

		EventBus.Instance.OnCountdownStarted += () => {
			int timeRemaining = 3;

			_newText = $"Wave starting in {timeRemaining} seconds";

			for(int i = 1; i < timeRemaining; i++) {
				Timer timer = new(1000 * (timeRemaining - i));
				
				int capturedI = i;

				timer.Elapsed += (_, _) => {
					_newText = $"Wave starting in {capturedI} seconds";
				};
				timer.AutoReset = false;
				timer.Enabled = true;
			}
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
