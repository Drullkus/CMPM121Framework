using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;
using Player;
using Relic;
using UI;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour {

    public Image level_selector;
    public GameObject button;
    public GameObject playerClassButton;
    public GameObject enemy;
    public GameObject defeatScreen;
    public SpawnPoint[] SpawnPoints;
    private Level selectedLevel;
    private PlayerClassData chosenPlayerClass;
    private int waveLevel = 1;
    private float waveDuration = 0;
    public PlayerController playerController;

    public delegate void OnWaveEndHandler();
    public event OnWaveEndHandler onWaveEnd;

    public void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start() {
        RelicManager.Instance.GetHashCode(); // Observe class so that it registers events
        EventBus.Instance.DoGameStarted();
        
        var levelsJsonAsset = Resources.Load<TextAsset>("levels");
        var levels = JsonConvert.DeserializeObject<List<Level>>(levelsJsonAsset.text);

        for (var index = 0; index < levels.Count; index++) {
            var level = levels[index];

            var selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, 130 - index * 40);
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(level);
        }

        var playerClassJsonAsset = Resources.Load<TextAsset>("classes");
        var playerClasses = JsonConvert.DeserializeObject<Dictionary<string, PlayerClassData>>(playerClassJsonAsset.text);

        for (var index = 0; index < playerClasses.Count; index++) {
            var playerClass = playerClasses.ElementAt(index);
            var playerClassData = playerClass.Value;
            if (index == 0) { // first is default class
                chosenPlayerClass = playerClassData;
            }

            var classSelector = Instantiate(playerClassButton, level_selector.transform);
            classSelector.transform.localPosition = new Vector3((index - (playerClasses.Count - 1) * 0.5f) * 80f, -50, -100);
            classSelector.GetComponent<ClassSelectorControl>().SetPlayerClass(playerClass.Key, playerClassData, () => chosenPlayerClass = playerClassData);
        }

        GameManager.Instance.enemySpawner = this;
    }

    void Update() {
        waveDuration += Time.deltaTime;
        
        GameManager.Instance.waveStatValues["waveDuration"] = (int)Math.Floor(waveDuration);
    }

    public void OnDestroy() {
        GameManager.Instance.ClearEnemies();
    }

    public void StartLevel(Level level) {
        this.selectedLevel = level;
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        playerController.StartLevel(chosenPlayerClass);
        StartCoroutine(this.StartWave());
    }

    public void NextWave() {
        if (selectedLevel.Waves == this.waveLevel) {
            this.Reset();
        }

        this.waveLevel++;

        if (selectedLevel.Waves == 0 || selectedLevel.Waves <= this.waveLevel) {
            this.playerController.OnNextWave(this.waveLevel);
            StartCoroutine(this.StartWave());
        }
    }


    IEnumerator StartWave() {
        waveDuration = 0;

        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--) {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        yield return this.SpawnWave();
        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
        onWaveEnd.Invoke();
    }

    IEnumerator SpawnWave() {
        List<Coroutine> coroutines = new List<Coroutine>();
        foreach (Spawn spawn in this.selectedLevel.Spawns)
        {
            // Asynchronously spawn each type of enemy
            coroutines.Add(StartCoroutine(SpawnEnemyType(spawn)));
        }

        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }
    }

    IEnumerator SpawnEnemyType(Spawn spawn) {
        spawn.CalculateForWave(this.waveLevel, out int totalEnemiesOfType, out int sequenceDelay);
        int enemyCount = 0;

        foreach (int spawnCount in spawn.GetSpawnBatches()) {
            for (int countInBatch = 0; countInBatch < spawnCount; countInBatch++) {
                SpawnPoint spawn_point = this.ChooseSpawnPoint(spawn.Location);
                Vector2 offset = UnityEngine.Random.insideUnitCircle * 1.8f;

                Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);

                this.SpawnEnemy(initial_position, spawn);

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

    void SpawnEnemy(Vector3 initial_position, Spawn spawn) {
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        EnemyStats enemyStats;

        if(!GameManager.Instance.enemyStats.TryGetValue(spawn.Enemy, out enemyStats)) {
            Debug.LogError($"tried to spawn enemy of type \"{spawn.Enemy}\" when no such enemy type exists!");
            return;
        }

        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemyStats.SpriteIndex);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        spawn.CalculateForNewSpawn(enemyStats, this.waveLevel, out int hp, out int speed, out int damage);
        en.hp = new Hittable(hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = speed;
        en.damage = damage;
        GameManager.Instance.AddEnemy(new_enemy);
    }

    public bool WavesCompleted() {
        return selectedLevel.Waves == this.waveLevel;
    }

}

