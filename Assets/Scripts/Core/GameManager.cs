using UnityEngine;

public class GameManager : MonoBehaviour {

	// we need to instantiate the WaveManager singleton
	// somewhere. this feels like an okay place for it,
	// but suggestions are welcome.
	// don't attempt to access this. ever. just get the
	// static WaveManager.Instance if you need a reference
	// to the WaveManager.
	private WaveManager _tempWaveManager = new();

}
