using System.Linq;
using UnityEngine;

namespace Relic {
    public class EffectDamageNearest : RelicEffect {
        private string hurtAmount;

		public EffectDamageNearest(RelicEffectData relicEffectData) {
            hurtAmount = relicEffectData.Amount;
        }

        public void ApplyEffect(GameObject subject) {
			GameObject closest = GetClosestEnemy(subject.transform.position);
			if (closest == null) return;

			var calculatedDamage = RPNEvaluator.RPNEvaluator.Evaluate(hurtAmount, new());
        	closest?.GetComponent<EnemyInstance>()?.Hit(new Damage(calculatedDamage, Damage.Type.PHYSICAL));
		}

		private GameObject GetClosestEnemy(Vector3 point) {
			var enemies = Object.FindObjectsByType<EnemyInstance>(FindObjectsSortMode.None);
            if (enemies == null || enemies.Length == 0) return null;
            if (enemies.Length == 1) return enemies[0].gameObject;
            return enemies.Select(inst => inst.gameObject).Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    	}
    }
}