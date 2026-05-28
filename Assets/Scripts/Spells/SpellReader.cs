using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class JsonSpellDamageData {
	
	[JsonProperty("amount")]
	public string amount;
	[JsonProperty("type")]
	public Damage.Type type;

}

public class JsonSpellProjectileData {
	
	[JsonProperty("trajectory")]
	public ProjectileTrajectory trajectory;
	[JsonProperty("speed")]
	public string speed;
	[JsonProperty("sprite")]
	public int sprite;

}

public class JsonSpellData {

	[JsonProperty("name")]
	public string name;
	[JsonProperty("description")]
	public string description;
	[JsonProperty("icon")]
	public int icon;

	[JsonProperty("damage")]
	public JsonSpellDamageData primaryDamage;
	[JsonProperty("secondary_damage")]
	public JsonSpellDamageData secondaryDamage;

	[JsonProperty("projectile")]
	public JsonSpellProjectileData primaryProjectile;
	[JsonProperty("secondary_projectile")]
	public JsonSpellProjectileData secondaryProjectile;

	[JsonProperty("cooldown")]
	public string cooldown;
	[JsonProperty("mana_cost")]
	public string manaCost;

	[JsonProperty("delay")]
	public string delay;
	[JsonProperty("N")]
	public string n;
	[JsonProperty("spray")]
	public string spray;

}

public class SpellReader {

	List<JsonSpellData> _jsonSpellData;

	private static SpellReader _instance;
	public static SpellReader Instance {
		get {
			if(_instance == null) {
				_instance = new();

				AssetManager.Instance.LoadJson("spells", (loadedJson) => {
					_instance._jsonSpellData = JsonConvert.DeserializeObject<List<JsonSpellData>>(loadedJson);
				});
			}

			return _instance;
		}
	}

	// TODO
	public Spell FetchRandomSpell() {
		return new Spell("debug spell", "debug spell", 0);
	}

	// TODO
	public SpellModifier FetchRandomModifier() {
		return new SpellModifier("debug modifier", "debug modifier");
	}
    
}
