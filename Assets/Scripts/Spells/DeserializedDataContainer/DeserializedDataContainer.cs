using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

public class DeserializedDataContainer {

	// common fields for spells and modifiers
	[JsonProperty("name")]
	public string name;
	[JsonProperty("description")]
	public string description;
	[JsonProperty("icon")]
	public int icon;

	// spell fields
	[JsonProperty("N")]
	public string n;
	[JsonProperty("spray")]
	public string spray;
	[JsonProperty("mana_cost")]
	public string manaCost;
	[JsonProperty("cooldown")]
	public string cooldown;
	[JsonProperty("damage")]
	public DeserializedDataContainer damage;
	[JsonProperty("secondary_damage")]
	public DeserializedDataContainer secondaryDamage;
	[JsonProperty("projectile")]
	public DeserializedDataContainer projectile;
	[JsonProperty("secondary_projectile")]
	public DeserializedDataContainer secondaryProjectile;
	
	// modifier-specific fields
	[JsonProperty("angle")]
	public string angle;
	[JsonProperty("delay")]
	public string delay;
	[JsonProperty("mana_adder")]
	public string manaAdder;
	[JsonProperty("mana_multiplier")]
	public string manaMultiplier;
	[JsonProperty("cooldown_multiplier")]
	public string cooldownMultiplier;
	[JsonProperty("projectile_trajectory")]
	public Trajectory projectileTrajectory;

	// spell damage fields
	[JsonProperty("amount")]
	public string amount;
	[JsonProperty("type")]
	public string type;

	// spell projectile fields
	[JsonProperty("sprite")]
	public int sprite;
	[JsonProperty("trajectory")]
	public Trajectory trajectory;
	[JsonProperty("speed")]
	public string speed;
	[JsonProperty("lifetime")]
	public string lifetime;

	private bool IsSpell() {
		return manaCost != "";
	}

	private bool IsSpellModifier() {
		return angle != "" ||
			delay != "" ||
			manaAdder != "" ||
			manaMultiplier != "" ||
			cooldownMultiplier != "" ||
			projectileTrajectory != Trajectory.UNDEFINED;
	}

	public static List<DeserializedDataContainer> ReadFromJson(string json) {
		Dictionary<string, DeserializedDataContainer> deserializedData = JsonConvert.DeserializeObject<Dictionary<string, DeserializedDataContainer>>(json);

		return deserializedData.Values.ToList();
	}
}


