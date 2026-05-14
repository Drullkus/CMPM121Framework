using Newtonsoft.Json;
using UnityEngine;

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
	public Spells.Trajectory projectileTrajectory;

	// spell damage fields
	[JsonProperty("amount")]
	public string amount;
	[JsonProperty("type")]
	public string type;

	// spell projectile fields
	[JsonProperty("sprite")]
	public int sprite;
	[JsonProperty("trajectory")]
	public Spells.Trajectory trajectory;
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
			projectileTrajectory != Spells.Trajectory.UNDEFINED;
	}

	public Spells.SpellData AsSpellData() {
		return new Spells.SpellData(this);
	}

	public Spells.ProjectileData AsProjectileData() {
		return new Spells.ProjectileData(this);
	}

	public Spells.Modifier AsModifier() {
		string target = "";
		string format = "";

		if(angle != "") {
			target = "angle";
			format = $"{angle}";
		} else if(delay != "") {
			target = "delay";
			format = $"{delay}";
		} else if(manaAdder != "") {
			target = "manaCost";
			format = $"{{value}} {manaAdder} +";
		} else if(manaMultiplier != "") {
			target = "manaCost";
			format = $"{{value}} {manaMultiplier} *";
		} else if(cooldownMultiplier != "") {
			target = "cooldownMultiplier";
			format = $"{{value}} {cooldownMultiplier} *";
		} else if(projectileTrajectory != Spells.Trajectory.UNDEFINED) {
			target = "projectileTrajectory";
			format = $"{projectileTrajectory}";
		}

		return new Spells.Modifier(name, description, target, format);
	}

	public static List<Spells.SpellData> baseSpellData = new();
	public static List<Spells.Modifier> modifiers = new();

	public static void ReadFromJson(string json) {
		Dictionary<string, DeserializedDataContainer> deserializedData = JsonConvert.DeserializeObject<Dictionary<string, DeserializedDataContainer>>(json);

		foreach(DeserializedDataContainer data in deserializedData.Values) {
			if(data.IsSpell()) {
				baseSpellData.Add(data.AsSpellData());
				continue;
			}

			if(data.IsSpellModifier()) {
				modifiers.Add(data.AsModifier());
				continue;
			}
		}
	}

}

