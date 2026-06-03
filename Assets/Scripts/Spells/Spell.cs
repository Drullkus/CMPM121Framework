using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

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

    public void Cast(int power, int wave) {
		Dictionary<string, int> castVariables = new() {
			[ "power" ] = power,
			[ "wave" ] = wave,
		};

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

		// TODO
		void ArcaneBolt() { }

		// TODO
		void MagicMissile() { }

		// TODO
		void ArcaneBlast() { }

		// TODO
		void ArcaneSpray() { }
	}

}
