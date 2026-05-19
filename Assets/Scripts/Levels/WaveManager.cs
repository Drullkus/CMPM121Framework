using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager {

	private Action<Spawn> _spawnAction;

	public event Action<List<Level>> OnLevelsDeserialized;

	public event Action OnWaveStart;
	public event Action OnWaveEnd;

	private int waveIndex = 0;

	public void Initialize() {
		AssetManager.Instance.LoadJson("classes", (loadedJson) => {
			List<Level> levels = JsonConvert.DeserializeObject<List<Level>>(loadedJson);

			OnLevelsDeserialized?.Invoke(levels);
		});
	}

	public void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StartWave(Level level) {
		GameManager.Instance.state = GameManager.GameState.COUNTDOWN;

		new Timer(3000).Elapsed += (_, _) => {
			GameManager.Instance.state = GameManager.GameState.INWAVE;

			foreach(Spawn spawn in level.Spawns) {
				EventBus.Instance.RequestSpawnScheduling(waveIndex, spawn);
			}
		};

		EventBus.Instance.OnAllEnemiesDefeated += EndWave;

		OnWaveStart?.Invoke();
	}

	// TODO
	public void EndWave() { }

	// TODO
	private void SetSpawnTimer(Spawn spawn) { }

}

