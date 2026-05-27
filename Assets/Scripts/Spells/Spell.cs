using System.Collections.Generic;
using System.Linq;

public enum SpellTraitType {
	TRAJECTORY,
	DAMAGE_TYPE,
	RPN_FLOAT,
}

public class SpellTrait {
	
	public SpellTraitType type;

	public string traitValue;

}

public class Spell {
	
	public string name;
	public string description;
	public int icon;

	private Dictionary<string, SpellTrait> _traits;

	// TODO
	public List<(string, SpellTrait)> GetTraits(List<string> traitNames) { return null; }

	public List<string> GetTraitNames() { return _traits.Keys.ToList(); }

	// TODO
    public void Cast() {}

}
