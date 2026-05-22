using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public enum GameState {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }

    public GameState state;

    public int countdown;
    public static GameManager Instance;

	private void Awake() {
		if(Instance != null) {
			if(Instance != this) {
				Destroy(this);
			}
			
			return;
		}

		Instance = this;
		_waveManager.Initialize();
	}

	private WaveManager _waveManager = new();

}
