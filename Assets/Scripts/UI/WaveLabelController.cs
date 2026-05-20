using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WaveLabelController : MonoBehaviour {

    private TextMeshProUGUI _label;

	private int _enemyCount = 0;

    void Start() {
        _label = GetComponent<TextMeshProUGUI>();

		EventBus.Instance.OnWaveStarted += (enemyCount) => {
			_enemyCount = enemyCount;
			_label.text = $"Enemies remaining: {_enemyCount}";
		};

		EventBus.Instance.OnEnemyDefeated += () => {
			_label.text = $"Enemies remaining: {--_enemyCount}";
		};

		EventBus.Instance.OnWaveEnded += () => {
			gameObject.SetActive(false);
		};
    }

}
