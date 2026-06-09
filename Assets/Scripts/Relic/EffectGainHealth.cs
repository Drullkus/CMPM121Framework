using UnityEngine;

namespace Relic {
    public class EffectGainHealth : RelicEffect {
        private string restoreAmount;

        public EffectGainHealth(RelicEffectData relicEffectData) {
            restoreAmount = relicEffectData.Amount;
        }
        
        public void ApplyEffect(GameObject subject) {
            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.RestoreHealth(RPNEvaluator.RPNEvaluator.Evaluate(restoreAmount, new()));
        }
    }
}