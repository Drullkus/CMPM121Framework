using UnityEngine;

namespace Relic.RelicEffect {
    public class GainManaEffect : RelicEffect {
        private readonly RelicEffectData _relicEffectData;
        private string restoreAmount;

        public GainManaEffect(RelicEffectData relicEffectData) {
            _relicEffectData = relicEffectData;
            restoreAmount = relicEffectData.Amount;
        }
        
        public void ApplyEffect(GameObject subject) {
            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.RestoreMana(RPNEvaluator.RPNEvaluator.Evaluate(restoreAmount, new()));
        }
    }
}