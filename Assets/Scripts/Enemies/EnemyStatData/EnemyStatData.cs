using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

public class EnemyStatData{

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

	public static bool CreateEnemyStatDictionaryFromJson(string statsJson, out Dictionary<string, EnemyStatData> enemyStatDictionary) {
		enemyStatDictionary = new Dictionary<string, EnemyStatData>();

		List<EnemyStatData> enemyStatsList = JsonConvert.DeserializeObject<List<EnemyStatData>>(statsJson);

		foreach(EnemyStatData statData in enemyStatsList) {
			if(enemyStatDictionary.TryAdd(statData.Name, statData)) { continue; }
			return false;
		}

		return true;
	}

}
