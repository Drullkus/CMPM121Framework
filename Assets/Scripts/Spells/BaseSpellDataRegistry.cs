using Newtonsoft.Json;
using UnityEngine;

private class DeserializedDataContainer {

	// common fields for spells and modifiers
	[DefaultValue("")]
	[JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _name;
	[DefaultValue("")]
	[JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _description;
	[DefaultValue(0)]
	[JsonProperty("icon", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly int _icon;

	// spell fields
	[DefaultValue("")]
	[JsonProperty("N", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _n;
	[DefaultValue("")]
	[JsonProperty("spray", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _spray;
	[DefaultValue("")]
	[JsonProperty("mana_cost", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _manaCost;
	[DefaultValue("")]
	[JsonProperty("cooldown", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _cooldown;
	[DefaultValue(null)]
	[JsonProperty("damage", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly deserializedDataContainer _damage;
	[DefaultValue(null)]
	[JsonProperty("secondary_damage", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly deserializedDataContainer _secondaryDamage;
	[DefaultValue(null)]
	[JsonProperty("projectile", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly deserializedDataContainer _projectile;
	[DefaultValue(null)]
	[JsonProperty("secondary_projectile", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly deserializedDataContainer _secondaryProjectile;
	
	// modifier-specific fields
	[DefaultValue("")]
	[JsonProperty("angle", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _angle;
	[DefaultValue("")]
	[JsonProperty("delay", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _delay;
	[DefaultValue("")]
	[JsonProperty("mana_adder", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _manaAdder;
	[DefaultValue("")]
	[JsonProperty("mana_multiplier", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _manaMultiplier;
	[DefaultValue("")]
	[JsonProperty("cooldown_multiplier", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _cooldownMultiplier;
	[DefaultValue("")]
	[JsonProperty("projectile_trajectory", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _projectileTrajectory;

	// spell damage fields
	[DefaultValue("")]
	[JsonProperty("amount", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _amount;
	[DefaultValue("")]
	[JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _type;

	// spell projectile fields
	[DefaultValue(0)]
	[JsonProperty("sprite", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly int _sprite;
	[DefaultValue("")]
	[JsonProperty("trajectory", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _trajectory;
	[DefaultValue("")]
	[JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly string _speed;
	[DefaultValue(0)]
	[JsonProperty("lifetime", DefaultValueHandling = DefaultValueHandling.Populate)]
	private readonly int _lifetime;

	public 

}

public class BaseSpellDataRegistry : MonoBehaviour {

	public static BaseSpellDataRegistry Instance;

	void Start() {
		Instance = this;

		TextAsset jsonFile = Resources.Load<TextAsset>("spells");
		Dictionary<string, DeserializedDataContainer> deserializedData = JsonConvert.DeserializeObject<Dictionary<string, DeserializedDataContainer>>(jsonFile.text);
	}

}

