using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine.SceneManagement;

public class WaveManager {

	private int _waveIndex = 0;
	private bool _onLastWave = false;

	private Level _level;

	private Action _enemyDefeatHandler;

	public void Initialize() {
		EventBus.Instance.OnDifficultyChosen += (level) => {
			_level = level;
			StartWave();
		};

		EventBus.Instance.OnWaveRequested += StartWave;

		new EnemySpawner().Initialize();
	}

	public void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StartWave() {
		if(_level.Waves == _waveIndex + 1) { _onLastWave = true; }

		int remainingEnemyCount = 0;

		int waveIndexReadOnly = _waveIndex;
		
		Timer timer = new Timer(3000);
		timer.Elapsed += (_, _) => {
			foreach(BatchSpawnData batchSpawnData in _level.batchSpawnData) {
				ExecutionQueue.Instance.Enqueue(() => {
					EventBus.Instance.RequestSpawnScheduling(waveIndexReadOnly, batchSpawnData);
				});

				batchSpawnData.CalculateForWave(waveIndexReadOnly, out int spawnCount, out _);
				remainingEnemyCount += spawnCount;
			}
			
			_enemyDefeatHandler = HandleEnemyDefeated;
			EventBus.Instance.OnEnemyDefeated += _enemyDefeatHandler;

			EventBus.Instance.StartWave(_waveIndex, remainingEnemyCount);
		};
		timer.AutoReset = false;
		timer.Enabled = true;

		EventBus.Instance.OnAllEnemiesDefeated += EndWave;

		return;

		// we need to capture `remainingEnemyCount` while still
		// being able to unsubscribe from OnEnemyDefeated later
		void HandleEnemyDefeated() {
			remainingEnemyCount--;
			if(remainingEnemyCount < 1) { EndWave(); }
		}
	}

	public void EndWave() {
		EventBus.Instance.OnEnemyDefeated -= _enemyDefeatHandler;
		EventBus.Instance.OnAllEnemiesDefeated -= EndWave;
		EventBus.Instance.EndWave();

		_waveIndex++;

		// TODO - handle last wave
	}

}

