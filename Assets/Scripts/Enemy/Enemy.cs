using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;

public class EnemyStats {

	[DefaultValue(0)]
	[JsonProperty("sprite", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int SpriteIndex { get; set; }

	[DefaultValue("unnamed enemy")]
	[JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Populate)]
	public string Name { get; set; }

	[DefaultValue(20)]
	[JsonProperty("hp", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int HP { get; set; }

	[DefaultValue(5)]
	[JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int Speed { get; set; }

	[DefaultValue(5)]
	[JsonProperty("damage", DefaultValueHandling = DefaultValueHandling.Populate)]
	public int Damage { get; set; }

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

public class Enemy {

	EnemyStats stats;

	Enemy(EnemyStats stats) {
		this.stats = stats;
	}

}
