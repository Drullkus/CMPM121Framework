using UnityEngine;

namespace Relic {
    public class EffectGainSpellpower : RelicEffect {
        private readonly string powerBonusKey;
        private string gainAmount;

        public EffectGainSpellpower(RelicEffectData relicEffectData) {
            powerBonusKey = relicEffectData.Description;
            
            gainAmount = relicEffectData.Amount;

            if (relicEffectData.Until != null && relicEffectData.Until != "") {
                RelicManager.Instance.GetEvent(relicEffectData.Until).Invoke(this.RemoveEffect);
            }
        }

        public void ApplyEffect(GameObject subject) {
            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.AddSpellpowerBonus(powerBonusKey, RPNEvaluator.RPNEvaluator.Evaluate(gainAmount, new()));
        }

        public void RemoveEffect(GameObject subject) {
            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.RemoveSpellpowerBonus(powerBonusKey);
        }
    }
}