using UnityEngine;

public class GameManager : MonoBehaviour {

	private WaveManager _waveManager = new();

	private void Awake() {
		_waveManager.Initialize();
	}

}
