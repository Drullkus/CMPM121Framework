using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager {

    public enum GameState {
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

    public Dictionary<string, EnemyStats> enemyStats;
    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }

    public Dictionary<string, int> waveStatValues = new(){
        [ "hitCount" ] = 0,
        [ "shotCount" ] = 0,
        [ "waveDuration" ] = 0,
    };
    private Dictionary<string, (string, string)> waveStatExpressions = new(){
        [ "waveDuration" ] = ("waveDuration", "Wave duration: {0} seconds"),
        [ "accuracy" ] = ("100 hitCount * shotCount /", "Shot accuracy: {0}%"),
    };

    public void AddEnemy(GameObject enemy) {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy) {
        enemies.Remove(enemy);
    }

    public void ClearEnemies() {
        enemies.Clear();
    }

    public GameObject GetClosestEnemy(Vector3 point) {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    public void ClearWaveStatValues() {
        foreach(string key in waveStatValues.Keys.ToArray()) {
            waveStatValues[key] = 0;
        }
    }

    public List<string> GetRandomStats() {
        List<int> possibleIndices = Enumerable.Range(0, waveStatExpressions.Count).ToList();

        List<string> stats = new();

        int baseLength = possibleIndices.Count;

        for(int i = 0; i < Math.Min(baseLength, 3); i++) {
            int indicesIndex = new System.Random().Next(0, possibleIndices.Count - 1);
            int expressionIndex = possibleIndices[indicesIndex];
            possibleIndices.RemoveAt(indicesIndex);

            (string, string) expression = waveStatExpressions.Values.ToArray()[expressionIndex];

            string description = expression.Item2;
            string value = RPNEvaluator.RPNEvaluator.Evaluate(expression.Item1, waveStatValues).ToString();

            string stat = String.Format(description, value);

            stats.Add(stat);
        }

        return stats;
    }

}
