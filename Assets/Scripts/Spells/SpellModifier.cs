using System;
using System.Collections.Generic;
using System.Linq;

public class SpellModifier {
    
	private Dictionary<string, Action<SpellTrait>> _traitModifications;

	public string name;
	public string description;

	public SpellModifier() { }

	public void SetData(string name, string description) {
		this.name = name;
		this.description = description;
	}

	public Spell ModifySpell(Spell spell) {
		foreach(
			(string traitName, SpellTrait trait) in
			spell.GetTraits(_traitModifications.Keys.ToList())
		) {
			_traitModifications[traitName].Invoke(trait);
		}

		return spell;
	}

}
