using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class Enemy {
	[JsonProperty("sprite", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int SpriteIndex = 0;

	[JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Populate)]
	public string Name = "unnamed enemy";
	[JsonProperty("hp", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int HP = 20;
	[JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int Speed = 5;
	[JsonProperty("damage", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int Damage = 5;

	public static Dictionary<string, Enemy> CreateEnemiesFromJson(string jsonPath) {
		TextAsset enemyJson = Resources.Load<TextAsset>(jsonPath);

		List<Enemy> enemies = JsonConvert.DeserializeObject<List<Enemy>>(enemyJson);

		Dictionary<string, Enemy> enemyDictionary = new Dictionary<string, Enemy>();

		foreach(Enemy enemy in enemies) {
			if(enemyDictionary.TryAdd(enemy.name, enemy)) { continue; }

			Debug.LogError($"{enemy.name} is defined multiple times in {jsonPath}!");
		}

		return enemyDictionary;
	}
}

