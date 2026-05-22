using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;

public class EnemySpawner {

    private SpawnPoint[] _spawnPoints;

	private Dictionary<string, EnemyStatData> _enemyStats = new();

	public void Initialize() {
		_spawnPoints = GameObject.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

		AssetManager.Instance.LoadJson("enemies", (loadedJson) => {
			List<EnemyStatData> stats = JsonConvert.DeserializeObject<List<EnemyStatData>>(loadedJson);

			foreach(EnemyStatData stat in stats) {
				_enemyStats[stat.Name] = stat;
			}
		});

		EventBus.Instance.OnSpawnSchedulingRequested += (waveIndex, spawn) => {
			spawn.CalculateForWave(waveIndex, out int total, out _);
			ScheduleSpawning(spawn, waveIndex, 0, total);
		};
	}

	private void ScheduleSpawning(BatchSpawnData batchSpawnData, int waveIndex, int batchIndex, int leftToSpawn) {
		int batchCount = batchSpawnData.GetSpawnBatchCount(batchIndex);
		int nextLeftToSpawn = leftToSpawn - batchCount;

		batchSpawnData.CalculateForWave(waveIndex, out _, out int delay);

		for(int i = 0; i < batchCount && i < leftToSpawn; i++) {
			SpawnPoint spawnPoint = ChooseSpawnPoint(batchSpawnData.Location);
			Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.8f;
			Vector2 initialPosition = spawnPoint.transform.position + (Vector3)offset;

			Timer timer = new(delay);
			timer.Elapsed += (_, _) => {
				ExecutionQueue.Instance.Enqueue(() => {
					EnemyInstance.Instantiate(
						_enemyStats[batchSpawnData.Enemy],
						(_) => {}
					);
				});

				if(nextLeftToSpawn > 0) {
					ScheduleSpawning(batchSpawnData, waveIndex, batchIndex + 1, nextLeftToSpawn);
				}
			};
			timer.AutoReset = false;
			timer.Enabled = true;
		}
	}

    SpawnPoint ChooseSpawnPoint(string filter) {
        string spawnKey = filter.Split()[^1].ToUpper();

        if (Enum.TryParse<SpawnPoint.SpawnName>(spawnKey, out var spawnType)) {
            List<SpawnPoint> spawnPointsFiltered = _spawnPoints.Where(p => p.kind == spawnType).ToList();

            return spawnPointsFiltered[UnityEngine.Random.Range(0, spawnPointsFiltered.Count)];
        }

        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)];
    }

}
