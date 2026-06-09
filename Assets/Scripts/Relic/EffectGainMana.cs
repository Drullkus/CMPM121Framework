using UnityEngine;

namespace Relic {
    public class EffectGainMana : RelicEffect {
        private readonly RelicEffectData _relicEffectData;
        private string restoreAmount;

        public EffectGainMana(RelicEffectData relicEffectData) {
            _relicEffectData = relicEffectData;
            restoreAmount = relicEffectData.Amount;
        }
        
        public void ApplyEffect(GameObject subject) {
            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.RestoreMana(RPNEvaluator.RPNEvaluator.Evaluate(restoreAmount, new()));
        }
    }
}