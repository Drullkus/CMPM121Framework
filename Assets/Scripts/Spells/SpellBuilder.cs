using System;

public class SpellBuilder {

	private static SpellBuilder _instance;
	public static SpellBuilder Instance {
		get {
			_instance ??= new();
			return _instance;
		}
	}

	public Spell GenerateSpell() {
		Spell spell = SpellReader.Instance.randomSpellBase;

		// SpellModifier modifier = SpellReader.Instance.randomSpellModifier;

		// return modifier.ModifySpell(spell);
		SpellModifier splitter = SpellReader.Instance.getSpellModifierFactory("splitter").Invoke();
		return splitter.ModifySpell(spell);
	}
    
}
