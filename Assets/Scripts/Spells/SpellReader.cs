using Newtonsoft.Json;
using System;

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
	[JsonProperty("secondarydamage")]
	public JsonSpellDamageData secondaryDamage;

	[JsonProperty("projectile")]
	public JsonSpellProjectileData primaryProjectile;
	[JsonProperty("secondaryprojectile")]
	public JsonSpellProjectileData secondaryProjectile;

	[JsonProperty("cooldown")]
	public string cooldown;
	[JsonProperty("manacost")]
	public string manaCost;

	[JsonProperty("delay")]
	public string delay;
	[JsonProperty("N")]
	public string n;
	[JsonProperty("spray")]
	public string spray;

}

public class SpellReader {

	// TODO
	public void FetchRandomSpell(Action<Spell> onSpellFetched) { }

	// TODO
	public void FetchRandomModifer(Action<SpellModifier> onModifierFetched) { }
    
}
