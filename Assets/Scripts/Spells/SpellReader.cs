using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	public int icon = -1;

	[JsonProperty("damage")]
	public JsonSpellDamageData primaryDamage;
	[JsonProperty("secondary_damage")]
	public string secondaryDamage;

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

	private Dictionary<string, Func<Spell>> _spellBaseFactories = new();
	private Dictionary<string, Func<SpellModifier>> _spellModifierFactories = new();

	private static SpellReader _instance;
	public static SpellReader Instance {
		get {
			if(_instance == null) {
				_instance = new();

				AssetManager.Instance.Deserialize<Dictionary<string, JsonSpellData>>("spells", _instance.RegisterSpells);
			}

			return _instance;
		}
	}

	public Spell randomSpellBase {
		get {
			return _spellBaseFactories.ElementAt(UnityEngine.Random.Range(0, _spellBaseFactories.Count)).Value();
		}
	}

	public SpellModifier randomSpellModifier {
		get {
			return _spellModifierFactories.ElementAt(UnityEngine.Random.Range(0, _spellModifierFactories.Count)).Value();
		}
	}

	public Func<Spell> getSpellBaseFactory(string spellName) {
		return _spellBaseFactories[spellName];
	}

	public Func<SpellModifier> getSpellModifierFactory(string spellName) {
		return _spellModifierFactories[spellName];
	}

	private void RegisterSpells(Dictionary<string, JsonSpellData> spellDatas) {
		_spellBaseFactories.Clear();
		_spellModifierFactories.Clear();

		if (spellDatas.Count == 0) {
			Debug.LogError("Zero Spell Data registered!");
		}

		foreach (var spellBasePrototype in spellDatas.Where(o => o.Value.icon > -1)) {
			_spellBaseFactories.Add(spellBasePrototype.Key, () => InstantiateSpellBase(spellBasePrototype.Value));
		}
		
		foreach (var spellModifierPrototype in spellDatas.Where(o => o.Value.icon == -1)) {
			_spellModifierFactories.Add(spellModifierPrototype.Key, () => InstantiateSpellModifier(spellModifierPrototype.Value));
		}
	}

	private Spell InstantiateSpellBase(JsonSpellData spellBasePrototype) {
		var newSpell = new Spell(spellBasePrototype.name, spellBasePrototype.description, spellBasePrototype.icon);
				
		// TODO Handle: N, damage, secondary_damage, spray, manacost, cooldown, projectile, secondary_projectile
				
		return newSpell;
	}

	private SpellModifier InstantiateSpellModifier(JsonSpellData spellModifierPrototype) {
		var newSpellModifier = new SpellModifier(spellModifierPrototype.name, spellModifierPrototype.description);
				
		// TODO handle all of the spell modifier info
				
		return newSpellModifier;
	}
    
}
