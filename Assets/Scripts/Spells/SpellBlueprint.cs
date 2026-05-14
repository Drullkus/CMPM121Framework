using System.Collections.Generic;

namespace Spells {

	public abstract class CastBehavior { public abstract void Cast(); }

	namespace CastBehaviors {

		public class Simple : CastBehavior {

			// TODO
			public override void Cast() {

			}

		}

	}

	public class SpellBlueprint {
		
		public string name;
		public string description;
		public int icon;

		private ProjectileBlueprint _primaryProjectileBlueprint;
		private ProjectileBlueprint _secondaryProjectileBlueprint;

		private CastBehavior _castBehavior;

		private Dictionary<string, Stat> _stats;

		public void Modify(Modifier modifier) {
			
		}

	}

}
