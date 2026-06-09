using UnityEngine;

namespace Relic {
    public class EffectNextSpellsFree : RelicEffect {
		private string freeSpellsLimit;
		private int freeSpellsRemaining;
        
		public EffectNextSpellsFree(RelicEffectData relicEffectData) {
			freeSpellsLimit = relicEffectData.Amount;

			EventBus.Instance.OnCastSpell += RefundMana;
		}

        public void ApplyEffect(GameObject subject) {
	        Debug.Log("EffectNextSpellsFree.ApplyEffect");
			freeSpellsRemaining = RPNEvaluator.RPNEvaluator.Evaluate(freeSpellsLimit, new());
        }

        public void RefundMana(GameObject subject) {
	        Debug.Log($"EffectNextSpellsFree.RefundMana freeSpellsRemaining: {freeSpellsRemaining}");
            if (freeSpellsRemaining <= 0) return;

            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.RestoreAllMana();

			freeSpellsRemaining--;
        }
    }
}