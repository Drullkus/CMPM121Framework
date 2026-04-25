using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class EnemyStats {
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

	public static Statuses.Status CreateEnemyStatDictionaryFromJson(string statsJson, out Dictionary<string, EnemyStats> enemyStatDictionary) {
		enemyStatDictionary = new Dictionary<string, EnemyStats>();

		List<EnemyStats> enemyStatsList = JsonConvert.DeserializeObject<List<EnemyStats>>(statsJson);

		foreach(EnemyStats enemyStats in enemyStatsList) {
			if(enemyStatDictionary.TryAdd(enemyStats.Name, enemyStats)) { continue; }
			return Statuses.Status.ENEMY_STAT_REDEFINITION;
		}

		return Statuses.Status.SUCCESS;
	}
}
