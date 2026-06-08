using UnityEngine;

public class GameManager : MonoBehaviour {

	private WaveManager _waveManager = new();

	private void Awake() {
		EventBus.Instance.DoGameStarted();
		_waveManager.Initialize();
	}

	public int getWave() {
		return _waveManager.getWave();
	}

}
