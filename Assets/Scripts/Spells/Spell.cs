using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[JsonConverter(typeof(StringEnumConverter))]
public enum ProjectileTrajectory {
	[EnumMember(Value = "homing")]
	HOMING,
	[EnumMember(Value = "spiraling")]
	SPIRALING,
	[EnumMember(Value = "straight")]
	STRAIGHT
}

public enum SpellTraitType {
	TRAJECTORY,
	DAMAGE_TYPE,
	RPN_FLOAT,
}

public class SpellTrait {
	
	public SpellTraitType type;

	public string traitValue;

	public SpellTrait(SpellTraitType type, string traitValue) {
		this.type = type;
		this.traitValue = traitValue;
	}

}

public class Spell {
	
	public string name;
	public string description;
	public int icon;

	private Dictionary<string, SpellTrait> _traits = new();

	public Spell(string name, string description, int icon) {
		this.name = name;
		this.description = description;
		this.icon = icon;
	}

	public void AddTrait(string traitName, SpellTrait trait) {
		// TODO - warn about duplicate `traitName`s
		_traits.TryAdd(traitName, trait);
	}

	public List<(string, SpellTrait)> GetTraits(List<string> traitNames) {
		List<(string, SpellTrait)> traits = new();
		
		foreach(string traitName in traitNames) {
			if(_traits.TryGetValue(traitName, out SpellTrait trait)) {
				traits.Add((traitName, trait));
			}
		}

		return traits;
	}

	public List<string> GetTraitNames() { return _traits.Keys.ToList(); }

    public void Cast(GameObject source, Vector2 direction, Team team, int power, int wave) {
		Dictionary<string, int> castVariables = new() {
			[ "power" ] = power,
			[ "wave" ] = wave,
		};

		if(_traits.TryGetValue("recoil.knockbackTimer", out SpellTrait timer)
			&& _traits.TryGetValue("recoil.knockbackForce", out SpellTrait force)
		) {
			EventBus.Instance.InvokeRecoil(
				source,
				RPNEvaluator.RPNEvaluator.Evaluatef(timer.traitValue, castVariables),
				RPNEvaluator.RPNEvaluator.Evaluatef(force.traitValue, castVariables)
			);

			Debug.Log("recoil invoked!");
		}

		switch(name) {
			case "Arcane Bolt":
				ArcaneBolt();
				break;
			case "Magic Missile":
				MagicMissile();
				break;
			case "Arcane Blast":
				ArcaneBlast();
				break;
			case "Arcane Spray":
				ArcaneSpray();
				break;
			default: break;
		}

		return;

		void ArcaneBolt() {
			ProjectileData projectileData = new ProjectileData()
				.SetRPNDictionary(castVariables)
				.SetTeam(team);

			if(_traits.TryGetValue("projectile.trajectory", out SpellTrait trajectory)) {
				projectileData.SetTrajectory(trajectory.traitValue);
			}

			if(_traits.TryGetValue("projectile.speed", out SpellTrait speed)) {
				projectileData.SetSpeed(speed.traitValue);
			}

			if(_traits.TryGetValue("projectile.lifetime", out SpellTrait lifetime)) {
				projectileData.SetLifetime(lifetime.traitValue);
			}

			if(_traits.TryGetValue("damage.amount", out SpellTrait damageAmount)) {
				projectileData.SetDamageAmount(damageAmount.traitValue);
			}

			Projectile.Spawn(source, direction, projectileData);
		}

		void MagicMissile() {
			ProjectileData projectileData = new ProjectileData()
				.SetRPNDictionary(castVariables)
				.SetTeam(team);

			if(_traits.TryGetValue("projectile.trajectory", out SpellTrait trajectory)) {
				projectileData.SetTrajectory(trajectory.traitValue);
			}

			if(_traits.TryGetValue("projectile.speed", out SpellTrait speed)) {
				projectileData.SetSpeed(speed.traitValue);
			}

			if(_traits.TryGetValue("projectile.lifetime", out SpellTrait lifetime)) {
				projectileData.SetSpeed(lifetime.traitValue);
			}

			if(_traits.TryGetValue("damage.amount", out SpellTrait damageAmount)) {
				projectileData.SetSpeed(damageAmount.traitValue);
			}

			Projectile.Spawn(source, direction, projectileData);
		}

		// TODO
		void ArcaneBlast() { }

		// TODO
		void ArcaneSpray() { }
	}

}
