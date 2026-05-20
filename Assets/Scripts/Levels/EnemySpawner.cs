using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;

    public void OnDestroy() {
        GameManager.Instance.ClearEnemies();
    }

    IEnumerator SpawnEnemyType(Spawn spawn, int waveIndex) {
        spawn.CalculateForWave(waveIndex, out int totalEnemiesOfType, out int sequenceDelay);
        int enemyCount = 0;

        foreach (int spawnCount in spawn.GetSpawnBatches()) {
            for (int countInBatch = 0; countInBatch < spawnCount; countInBatch++) {
                SpawnPoint spawn_point = this.ChooseSpawnPoint(spawn.Location);
                Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.8f;

                Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);

                this.SpawnEnemy(initial_position, spawn, waveIndex);

                if (++enemyCount >= totalEnemiesOfType) {
                    yield break;
                }
            }

            yield return new WaitForSeconds(sequenceDelay);
        }

        yield return null;
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
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        EnemyStats enemyStats;

        if(!GameManager.Instance.enemyStats.TryGetValue(spawn.Enemy, out enemyStats)) {
            Debug.LogError($"tried to spawn enemy of type \"{spawn.Enemy}\" when no such enemy type exists!");
            return;
        }

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemyStats.SpriteIndex);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        spawn.CalculateForNewSpawn(enemyStats, waveIndex, out int hp, out int speed, out int damage);
        en.hp = new Hittable(hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = speed;
        en.damage = damage;
        GameManager.Instance.AddEnemy(new_enemy);
    }

}

