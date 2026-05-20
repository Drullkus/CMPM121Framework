using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;

public class EnemySpawner {

    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;

	private Dictionary<string, EnemyStats> _enemyStats = new();

	public void Initialize() {
		SpawnPoints = GameObject.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

		AssetManager.Instance.LoadJson("enemies", (loadedJson) => {
			List<EnemyStats> stats = JsonConvert.DeserializeObject<List<EnemyStats>>(loadedJson);

			foreach(EnemyStats stat in stats) {
				_enemyStats[stat.Name] = stat;
			}
		});

		EventBus.Instance.OnSpawnSchedulingRequested += (waveIndex, spawn) => {
			spawn.CalculateForWave(waveIndex, out int total, out _);
			ScheduleSpawning(spawn, waveIndex, 0, total);
		};
	}

	private void ScheduleSpawning(Spawn spawn, int waveIndex, int batchIndex, int leftToSpawn) {
		int batchCount = spawn.GetSpawnBatchCount(batchIndex);
		int nextLeftToSpawn = leftToSpawn - batchCount;

		spawn.CalculateForWave(waveIndex, out _, out int delay);

		for(int i = 0; i < batchCount && i < leftToSpawn; i++) {
			SpawnPoint spawnPoint = ChooseSpawnPoint(spawn.Location);
			Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.8f;
			Vector2 initialPosition = spawnPoint.transform.position + (Vector3)offset;

			Timer timer = new(delay);
			timer.Elapsed += (_, _) => {
				SpawnEnemy(initialPosition, spawn, waveIndex);

				if(nextLeftToSpawn > 0) {
					ScheduleSpawning(spawn, waveIndex, batchIndex + 1, nextLeftToSpawn);
				}
			};
			timer.AutoReset = false;
			timer.Enabled = true;
		}
	}

    SpawnPoint ChooseSpawnPoint(string filter) {
        string spawnKey = filter.Split()[^1].ToUpper();

        if (Enum.TryParse<SpawnPoint.SpawnName>(spawnKey, out var spawnType)) {
            List<SpawnPoint> spawnPointsFiltered = SpawnPoints.Where(p => p.kind == spawnType).ToList();

            return spawnPointsFiltered[UnityEngine.Random.Range(0, spawnPointsFiltered.Count)];
        }

        return SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];
    }

    void SpawnEnemy(Vector3 initial_position, Spawn spawn, int waveIndex) {
        GameObject new_enemy = GameObject.Instantiate(enemy, initial_position, Quaternion.identity);

        if(!_enemyStats.TryGetValue(spawn.Enemy, out EnemyStats enemyStats)) {
            Debug.LogError($"tried to spawn enemy of type \"{spawn.Enemy}\" when no such enemy type exists!");
            return;
        }

        new_enemy.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrieveEnemySprite(enemyStats.SpriteIndex);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        spawn.CalculateForNewSpawn(enemyStats, waveIndex, out int hp, out int speed, out int damage);
        en.hp = new Hittable(hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = speed;
        en.damage = damage;
    }

}

