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

		SpellModifier recoil = SpellReader.Instance.getSpellModifierFactory("recoil").Invoke();
		// SpellModifier modifier = SpellReader.Instance.randomSpellModifier;

		return recoil.ModifySpell(spell);
	}
    
}
