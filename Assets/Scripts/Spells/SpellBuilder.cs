using System;

public class SpellBuilder {

	public Spell GenerateSpell() {
		Spell spell = SpellReader.Instance.FetchRandomSpell();
		SpellModifier modifier = SpellReader.Instance.FetchRandomModifier();

		return modifier.ModifySpell(spell);
	}
    
}
