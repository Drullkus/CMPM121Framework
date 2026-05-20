using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager {

	public event Action<List<Level>> OnLevelsDeserialized;

	private int _waveIndex = 0;
	private bool _onLastWave = false;

	private Action _enemyDefeatHandler;

	public void Initialize() {
		AssetManager.Instance.LoadJson("levels", (loadedJson) => {
			List<Level> levels = JsonConvert.DeserializeObject<List<Level>>(loadedJson);

			OnLevelsDeserialized?.Invoke(levels);
		});
	}

	public void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StartWave(Level level) {
		if(level.Waves == _waveIndex + 1) { _onLastWave = true; }

		GameManager.Instance.state = GameManager.GameState.COUNTDOWN;

		int remainingEnemyCount = 0;
		
		new Timer(3000).Elapsed += (_, _) => {
			GameManager.Instance.state = GameManager.GameState.INWAVE;

			foreach(Spawn spawn in level.Spawns) {
				EventBus.Instance.RequestSpawnScheduling(_waveIndex, spawn);

				spawn.CalculateForWave(_waveIndex, out int spawnCount, out _);
				remainingEnemyCount += spawnCount;
			}

			_enemyDefeatHandler = HandleEnemyDefeated;
			EventBus.Instance.OnEnemyDefeated += _enemyDefeatHandler;
		};

		EventBus.Instance.OnAllEnemiesDefeated += EndWave;

		EventBus.Instance.StartWave();

		return;

		// we need to capture `remainingEnemyCount` while still
		// being able to unsubscribe from OnEnemyDefeated later
		void HandleEnemyDefeated() {
			if(remainingEnemyCount < 1) { EndWave(); }
		}
	}

	public void EndWave() {
		EventBus.Instance.OnEnemyDefeated -= _enemyDefeatHandler;
		EventBus.Instance.OnAllEnemiesDefeated -= EndWave;
		EventBus.Instance.EndWave();

		_waveIndex++;

		// TODO - move to GameManager so it can control its own state!
		GameManager.Instance.state = GameManager.GameState.WAVEEND;

		// TODO - handle last wave
	}

}

