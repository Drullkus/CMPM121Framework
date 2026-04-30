using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Mathematics;
using System;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;
    private Level selectedLevel;
    private int waveLevel = 1;
    private float waveDuration = 0;

    public delegate void OnWaveEndHandler();
    public event OnWaveEndHandler onWaveEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("levels");
        List<Level> levels = JsonConvert.DeserializeObject<List<Level>>(jsonAsset.text);

        for (int index = 0; index < levels.Count; index++)
        {
            Level level = levels[index];

            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, 130 - index * 40);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(level);
        }

        GameManager.Instance.enemySpawner = this;
    }

    // Update is called once per frame
    void Update()
    {
        waveDuration += Time.deltaTime;
        
        GameManager.Instance.waveStatValues["waveDuration"] = (int)Math.Floor(waveDuration);
    }

    public void StartLevel(Level level)
    {
        this.selectedLevel = level;
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        StartCoroutine(this.StartWave());
    }

    public void NextWave()
    {
        this.waveLevel++;

        if (selectedLevel.Waves == 0 || selectedLevel.Waves <= this.waveLevel)
        {
            StartCoroutine(this.StartWave());
        }
    }


    IEnumerator StartWave()
    {
        waveDuration = 0;

        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        yield return this.SpawnWave();
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
        onWaveEnd.Invoke();
    }

    IEnumerator SpawnWave()
    {
        foreach (Spawn spawn in this.selectedLevel.Spawns)
        {
            // Asynchronously spawn each type of enemy
            StartCoroutine(SpawnEnemyType(spawn));
        }

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator SpawnEnemyType(Spawn spawn)
    {
        int totalEnemiesOfType = spawn.GetCountInWave(this.waveLevel);
        int enemyCount = 0;

        foreach (int spawnCount in spawn.GetSpawnBatches())
        {
            for (int countInBatch = 0; countInBatch < spawnCount; countInBatch++)
            {
                SpawnPoint spawn_point = this.ChooseSpawnPoint(spawn.Location);
                Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.8f;

                Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);

                this.SpawnEnemy(initial_position, spawn);

                if (++enemyCount >= totalEnemiesOfType)
                {
                    yield break;
                }
            }

            yield return new WaitForSeconds(spawn.GetDelayInWave(this.waveLevel));
        }

        yield return null;
    }

    SpawnPoint ChooseSpawnPoint(string filter)
    {
        string spawnKey = filter.Split()[^1].ToUpper();

        if (Enum.TryParse<SpawnPoint.SpawnName>(spawnKey, out var spawnType))
        {
            List<SpawnPoint> spawnPointsFiltered = SpawnPoints.Where(p => p.kind == spawnType).ToList();

            return spawnPointsFiltered[UnityEngine.Random.Range(0, spawnPointsFiltered.Count)];
        }

        return SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];
    }

    void SpawnEnemy(Vector3 initial_position, Spawn spawn)
    {
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        EnemyStats enemyStats;

        if(!GameManager.Instance.enemyStats.TryGetValue(spawn.Enemy, out enemyStats)) {
            Debug.LogError($"tried to spawn enemy of type \"{spawn.Enemy}\" when no such enemy type exists!");
            return;
        }

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemyStats.SpriteIndex);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(spawn.GetHpInWave(enemyStats.HP, waveLevel), Hittable.Team.MONSTERS, new_enemy);
        en.speed = spawn.GetSpeedInWave(enemyStats.Speed, waveLevel);
        en.damage = spawn.GetDamageInWave(enemyStats.Damage, waveLevel);
        GameManager.Instance.AddEnemy(new_enemy);
    }
}
