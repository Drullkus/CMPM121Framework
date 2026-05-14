using Newtonsoft.Json;
using UnityEngine;

public class DeserializedDataContainer {

	// common fields for spells and modifiers
	[JsonProperty("name")]
	public readonly string name;
	[JsonProperty("description")]
	public readonly string description;
	[JsonProperty("icon")]
	public readonly int icon;

	// spell fields
	[JsonProperty("N")]
	public readonly string n;
	[JsonProperty("spray")]
	public readonly string spray;
	[JsonProperty("manacost")]
	public readonly string manaCost;
	[JsonProperty("cooldown")]
	public readonly string cooldown;
	[JsonProperty("damage")]
	public readonly DeserializedDataContainer damage;
	[JsonProperty("secondarydamage")]
	public readonly DeserializedDataContainer secondaryDamage;
	[JsonProperty("projectile")]
	public readonly DeserializedDataContainer projectile;
	[JsonProperty("secondaryprojectile")]
	public readonly DeserializedDataContainer secondaryProjectile;
	
	// modifier-specific fields
	[JsonProperty("angle")]
	public readonly string angle;
	[JsonProperty("delay")]
	public readonly string delay;
	[JsonProperty("manaadder")]
	public readonly string manaAdder;
	[JsonProperty("manamultiplier")]
	public readonly string manaMultiplier;
	[JsonProperty("cooldownmultiplier")]
	public readonly string cooldownMultiplier;
	[JsonProperty("projectiletrajectory")]
	public readonly string projectileTrajectory;

	// spell damage fields
	[JsonProperty("amount")]
	public readonly string amount;
	[JsonProperty("type")]
	public readonly string type;

	// spell projectile fields
	[JsonProperty("sprite")]
	public readonly int sprite;
	[JsonProperty("trajectory")]
	public readonly Spells.Trajectory trajectory;
	[JsonProperty("speed")]
	public readonly string speed;
	[JsonProperty("lifetime")]
	public readonly string lifetime;

	public Spells.SpellData AsSpellData() {
		return new Spells.SpellData(this);
	}

	public Spells.ProjectileData AsProjectileData() {
		return new Spells.ProjectileData(this);
	}

}

