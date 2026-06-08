using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellModifier {
    
	private Dictionary<string, string> _traitModifications = new();

	public string name;
	public string description;

	public SpellModifier(string name, string description) {
		this.name = name;
		this.description = description;
	}

	public void AddModifier(string key, string modifier) {
		_traitModifications[key] = modifier;
	}

	public Spell ModifySpell(Spell spell) {
		spell.name = $"{name} {spell.name}";
		spell.description = $"{spell.description}\n{name}: {description}";

		if(name.ToLower() == "recoil") {
			spell.AddTrait("recoil.knockbackTimer", new(SpellTraitType.RPN_FLOAT, ""));
			spell.AddTrait("recoil.knockbackForce", new(SpellTraitType.RPN_FLOAT, ""));
		}

		if(name.ToLower() == "doubled") {
			spell.AddTrait("doubler.delay", new(SpellTraitType.RPN_FLOAT, ""));
		}

		if(name.ToLower() == "angle") {
			spell.AddTrait("splitter.angle", new(SpellTraitType.RPN_FLOAT, ""));
		}

		foreach(
			(string traitName, SpellTrait trait) in
			spell.GetTraits(_traitModifications.Keys.ToList())
		) {
			string oldValue = trait.traitValue;
			trait.traitValue = String.Format(_traitModifications[traitName], oldValue);
		}

		return spell;
	}

}
