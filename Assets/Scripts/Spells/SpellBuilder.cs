using System;

public class SpellBuilder {

	public Spell GenerateSpell() {
		Spell spell = SpellReader.Instance.randomSpellBase;
		SpellModifier modifier = SpellReader.Instance.randomSpellModifier;

		return modifier.ModifySpell(spell);
	}
    
}
