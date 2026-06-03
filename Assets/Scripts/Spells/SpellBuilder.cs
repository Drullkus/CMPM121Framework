using System;

public class SpellBuilder {

	public Spell GenerateSpell(out string description) {
		Spell spell = SpellReader.Instance.randomSpellBase;
		SpellModifier modifier = SpellReader.Instance.randomSpellModifier;

		description = $"{modifier.name}: {modifier.description}";

		return modifier.ModifySpell(spell);
	}
    
}
