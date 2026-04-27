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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("levels");
        Debug.Log($"jsonAsset.text: {jsonAsset.text}");
        List<Level> levels = JsonConvert.DeserializeObject<List<Level>>(jsonAsset.text);

        for (int index = 0; index < levels.Count; index++)
        {
            Level level = levels[index];

            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, 130 - index * 40);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(level);

            Debug.Log($"LEVEL: {level.Name}, {level.Waves}, {level.Spawns}");
            foreach (Spawn spawn in level.Spawns)
            {
                Debug.Log($"SPAWN: {spawn.Enemy}, {spawn.Count}, {spawn.Hp}, {spawn.Speed}, {spawn.Damage}, {spawn.Delay}, {spawn.Location}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(Level level)
    {
        Debug.Log(level.Name);
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

        for (int enemyCount = 1; enemyCount <= totalEnemiesOfType; enemyCount++)
        {
            SpawnPoint spawn_point = this.ChooseSpawnPoint(spawn.Location);
            Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.8f;

            Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);

            this.SpawnEnemy(initial_position);

            // TODO handle sequencing logic
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

    void SpawnEnemy(Vector3 initial_position)
    {
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(0);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        en.hp = new Hittable(50, Hittable.Team.MONSTERS, new_enemy);
        en.speed = 10;
        GameManager.Instance.AddEnemy(new_enemy);
    }
}
