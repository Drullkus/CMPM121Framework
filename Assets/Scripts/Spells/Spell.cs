using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
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

	private readonly string _baseName;

	private Dictionary<string, SpellTrait> _traits = new();

	public Spell(string name, string description, int icon) {
		this.name = name;
		this.description = description;
		this.icon = icon;

		_baseName = name;
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
		}

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

		if(_traits.TryGetValue("doubler.delay", out SpellTrait doublerDelay)) {
			float delay = RPNEvaluator.RPNEvaluator.Evaluatef(doublerDelay.traitValue, castVariables);

			Timer secondCastTimer = new(delay * 1000.0f);
			secondCastTimer.Elapsed += (_, _) => {
				ExecutionQueue.Instance.Enqueue(_Cast);
			};
			secondCastTimer.AutoReset = false;
			secondCastTimer.Enabled = true;
		}

		_Cast();

		return;

		void _Cast() {
			if(source == null) { return; }
			if(projectileData == null) { return; }

			switch(_baseName) {
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
		}

		void ArcaneBolt() {
			Projectile.Spawn(source, source.transform.position, direction, projectileData);
		}

		void MagicMissile() {
			Projectile.Spawn(source, source.transform.position, direction, projectileData);
		}

		void ArcaneBlast() {
			ProjectileData secondaryData = new ProjectileData()
				.SetRPNDictionary(castVariables)
				.SetTeam(team);

			if(_traits.TryGetValue("projectile.secondary.trajectory", out SpellTrait trajectory)) {
				secondaryData.SetTrajectory(trajectory.traitValue);
			} else if(_traits.TryGetValue("projectile.trajectory", out trajectory)) {
				secondaryData.SetTrajectory(trajectory.traitValue);
			}
	
			if(_traits.TryGetValue("projectile.secondary.speed", out SpellTrait speed)) {
				secondaryData.SetSpeed(speed.traitValue);
			}
	
			if(_traits.TryGetValue("projectile.secondary.lifetime", out SpellTrait lifetime)) {
				secondaryData.SetLifetime(lifetime.traitValue);
			}
	
			if(_traits.TryGetValue("damage.secondary.amount", out SpellTrait damageAmount)) {
				secondaryData.SetDamageAmount(damageAmount.traitValue);
			}

			int n = (int)RPNEvaluator.RPNEvaluator.Evaluatef(_traits["n"].traitValue, castVariables);

			projectileData.SetOnHit((_, _, coordinates) => {
				for(int i = 0; i < n; i++) {
					float angle = (float)i * 2.0f * Mathf.PI / (float)n;

					Projectile newProjectile = Projectile.Spawn(
						source,
						coordinates,
						new(Mathf.Cos(angle), Mathf.Sin(angle)),
						secondaryData
					);
				}
			});

			Projectile.Spawn(source, source.transform.position, direction, projectileData);
		}

		void ArcaneSpray() {
			int n = (int)RPNEvaluator.RPNEvaluator.Evaluatef(_traits["n"].traitValue, castVariables);
			float delay = RPNEvaluator.RPNEvaluator.Evaluatef(_traits["spray"].traitValue, castVariables);

			for(int i = 1; i < n; i++) {
				Timer timer = new(delay * 1000.0f);
				timer.Elapsed += (_, _) => {
					ExecutionQueue.Instance.Enqueue(() => {
						Projectile.Spawn(source, source.transform.position, direction, projectileData);
					});
				};
				timer.AutoReset = false;
				timer.Enabled = true;
			}

			Projectile.Spawn(source, source.transform.position, direction, projectileData);
		}
	}

}
