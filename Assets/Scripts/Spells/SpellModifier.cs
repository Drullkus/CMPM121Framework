using System;
using System.Collections.Generic;
using System.Linq;

public class SpellModifier {
    
	private Dictionary<string, Action<SpellTrait>> _traitModifications;

	public void ModifySpell(Spell spell) {
		foreach(
			(string traitName, SpellTrait trait) in
			spell.GetTraits(_traitModifications.Keys.ToList())
		) {
			_traitModifications[traitName].Invoke(trait);
		}
	}

}
