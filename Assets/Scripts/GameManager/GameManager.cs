using Relic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private WaveManager _waveManager = new();

	private void Awake() {
		RelicManager.Instance.GetHashCode(); // Observe class so that it registers events
		EventBus.Instance.DoGameStarted();
		_waveManager.Initialize();
	}

	public int getWave() {
		return _waveManager.getWave();
	}

}
