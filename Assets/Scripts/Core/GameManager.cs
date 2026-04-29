using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager 
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {
        get {
            if (theInstance == null)
                theInstance = new GameManager();
            return theInstance;
        }
    }

    public GameObject player;
    
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    // AVERT YOUR EYES...
    // AHEAD LIES THE WORST CODE I THINK I'VE EVER WRITTEN
    // TODO rearchitect the project so that this isn't how
    //      we have to do things :/
    private RewardScreenManager _rewardScreenManager;
    private EnemySpawner _enemySpawner;

    public RewardScreenManager rewardScreenManager
    {
        get => _rewardScreenManager;
        set
        {
            _rewardScreenManager = value;
            if(_enemySpawner) { _enemySpawner.onWaveEnd += () => { _rewardScreenManager.ShowRewardScreen(GetRandomStats()); }; }
        }
    }

    public EnemySpawner enemySpawner
    {
        get => _enemySpawner;
        set
        {
            _enemySpawner = value;
            if(_rewardScreenManager) { _enemySpawner.onWaveEnd += () => { _rewardScreenManager.ShowRewardScreen(GetRandomStats()); }; }
        }
    }

    public Dictionary<string, EnemyStats> enemyStats;
    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    public List<string> GetRandomStats()
    {
        return new List<string>{ "stat example 1", "stat example 2", "stat example 3" };
    }

    private GameManager()
    {
		enemies = new List<GameObject>();
        TextAsset jsonFile = Resources.Load<TextAsset>("enemies");
        Statuses.Status res = EnemyStats.CreateEnemyStatDictionaryFromJson(jsonFile.text, out enemyStats);
        if(res != Statuses.Status.SUCCESS) { Debug.LogError(res.StatusString()); }
        else {
            foreach(EnemyStats stats in enemyStats.Values) {
                Debug.Log(stats.Name);
            }
        }
    }
}
