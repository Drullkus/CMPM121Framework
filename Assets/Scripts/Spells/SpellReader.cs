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
	[JsonProperty("lifetime")]
	public string lifetime;
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

	// modifier-specific fieds
	[JsonProperty("damage_multiplier")]
	public string damageMultiplier;
	[JsonProperty("mana_multiplier")]
	public string manaMultiplier;
	[JsonProperty("speed_multiplier")]
	public string speedMultiplier;
	[JsonProperty("cooldown_multiplier")]
	public string cooldownMultiplier;
	[JsonProperty("angle")]
	public string angle;
	[JsonProperty("projectile_trajectory")]
	public string projectileTrajectory;
	[JsonProperty("mana_adder")]
	public string manaAdder;
	[JsonProperty("knockback_timer")]
	public string recoilKnockbackTimer;
	[JsonProperty("knockback_force")]
	public string recoilKnockbackForce;

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
				
		List<(string, SpellTrait)> traits = new();
		if(spellBasePrototype.primaryDamage != null) {
			traits.Add(("damage.amount", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.primaryDamage.amount)));
			traits.Add(("damage.type", new(SpellTraitType.DAMAGE_TYPE, spellBasePrototype.primaryDamage.type.ToString())));
		}

		if(spellBasePrototype.secondaryDamage != null) {
			traits.Add(("damage.secondary.amount", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.primaryDamage.amount)));
			traits.Add(("damage.secondary.type", new(SpellTraitType.DAMAGE_TYPE, spellBasePrototype.primaryDamage.type.ToString())));
		}

		if(spellBasePrototype.primaryProjectile != null) {
			traits.Add(("projectile.speed", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.primaryProjectile.speed)));
			traits.Add(("projectile.trajectory", new(SpellTraitType.TRAJECTORY, spellBasePrototype.primaryProjectile.trajectory.ToString())));

			if(spellBasePrototype.primaryProjectile.lifetime != null) {
				traits.Add(("projectile.lifetime", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.primaryProjectile.lifetime)));
			}
		}

		if(spellBasePrototype.secondaryProjectile != null) {
			traits.Add(("projectile.secondary.speed", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.secondaryProjectile.speed)));
			traits.Add(("projectile.secondary.trajectory", new(SpellTraitType.TRAJECTORY, spellBasePrototype.secondaryProjectile.trajectory.ToString())));

			if(spellBasePrototype.secondaryProjectile.lifetime != null) {
				traits.Add(("projectile.secondary.lifetime", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.secondaryProjectile.lifetime)));
			}
		}

		if(spellBasePrototype.cooldown!= null) { traits.Add(("cooldown", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.cooldown))); }
		if(spellBasePrototype.manaCost!= null) { traits.Add(("manaCost", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.manaCost))); }
		if(spellBasePrototype.n!= null) { traits.Add(("n", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.n))); }
		if(spellBasePrototype.spray!= null) { traits.Add(("spray", new(SpellTraitType.RPN_FLOAT, spellBasePrototype.spray))); }

		foreach((string key, SpellTrait trait) in traits) {
			newSpell.AddTrait(key, trait);
		}

		return newSpell;
	}

	private SpellModifier InstantiateSpellModifier(JsonSpellData spellModifierPrototype) {
		var newSpellModifier = new SpellModifier(spellModifierPrototype.name, spellModifierPrototype.description);
				
		List<(string, SpellTrait)> traits = new();
		if(spellModifierPrototype.damageMultiplier != null) { traits.Add(("damage.amount", new(SpellTraitType.RPN_FLOAT, "{0} " + spellModifierPrototype.damageMultiplier + " *"))); }
		if(spellModifierPrototype.manaMultiplier != null) { traits.Add(("manaCost", new(SpellTraitType.RPN_FLOAT, "{0} " + spellModifierPrototype.manaMultiplier + " *"))); }
		if(spellModifierPrototype.speedMultiplier != null) { traits.Add(("speed", new(SpellTraitType.RPN_FLOAT, "{0} " + spellModifierPrototype.speedMultiplier + " *"))); }
		if(spellModifierPrototype.cooldownMultiplier != null) { traits.Add(("cooldown", new(SpellTraitType.RPN_FLOAT, "{0} " + spellModifierPrototype.cooldownMultiplier + " *"))); }
		if(spellModifierPrototype.angle != null) { traits.Add(("angle", new(SpellTraitType.RPN_FLOAT, spellModifierPrototype.angle))); }
		if(spellModifierPrototype.projectileTrajectory != null)  { traits.Add(("projectile.trajectory", new(SpellTraitType.TRAJECTORY, spellModifierPrototype.projectileTrajectory))); }
		if(spellModifierPrototype.manaAdder != null) { traits.Add(("manaAdder", new(SpellTraitType.RPN_FLOAT, "{0} " + spellModifierPrototype.manaAdder + " +"))); }
		if(spellModifierPrototype.recoilKnockbackTimer != null) { traits.Add(("recoil.knockbackTimer", new(SpellTraitType.RPN_FLOAT, spellModifierPrototype.recoilKnockbackTimer))); }
		if(spellModifierPrototype.recoilKnockbackForce != null) { traits.Add(("recoil.knockbackForce", new(SpellTraitType.RPN_FLOAT, spellModifierPrototype.recoilKnockbackForce))); }
		if(spellModifierPrototype.delay != null) { traits.Add(("doubler.delay", new(SpellTraitType.RPN_FLOAT, spellModifierPrototype.delay))); }
		if(spellModifierPrototype.angle != null) { traits.Add(("splitter.angle", new(SpellTraitType.RPN_FLOAT, spellModifierPrototype.angle))); }

		foreach((string key, SpellTrait trait) in traits) {
			newSpellModifier.AddModifier(key, trait.traitValue);
		}

		return newSpellModifier;
	}
    
}
