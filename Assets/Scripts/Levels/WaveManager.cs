using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager {

	private Action<Spawn> _spawnAction;

	public event Action<List<Level>> OnLevelsDeserialized;

	public void Initialize() {
		AssetManager.Instance.LoadJson("classes", (loadedJson) => {
			List<Level> levels = JsonConvert.DeserializeObject<List<Level>>(loadedJson);

			OnLevelsDeserialized.Invoke(levels);
		});
	}

	public void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// TODO
	public void StartWave(Level level) { }

	// TODO
	public void EndWave() { }

	// TODO
	private void SetSpawnTimer(Spawn spawn) { }

}

