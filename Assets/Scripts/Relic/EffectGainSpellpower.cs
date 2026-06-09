using UnityEngine;

namespace Relic {
    public class GainSpellpowerEffect : RelicEffect {
        private readonly string powerBonusKey;
        private string gainAmount;

        public GainSpellpowerEffect(RelicEffectData relicEffectData) {
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