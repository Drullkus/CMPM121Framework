
namespace Spells {
	public class SpellModifier {
	
		private SpellStatPath _modifiedStat;

		private string _modificationFormat;

		// TODO
		void Transform(Spell.Spell spell) {
			spell.ModifyStat(_modifiedStat, _modificationFormat);
		}
	}
}

