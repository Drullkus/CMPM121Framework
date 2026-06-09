using UnityEngine;

namespace Relic {
    public class GainSpellpowerEffect : RelicEffect {
        private readonly RelicEffectData _relicEffectData;
        private string gainAmount;

        public GainSpellpowerEffect(RelicEffectData relicEffectData) {
            _relicEffectData = relicEffectData;
            
            gainAmount = relicEffectData.Amount;

            if (relicEffectData.Until != null && relicEffectData.Until != "") {
                RelicManager.Instance.GetEvent(relicEffectData.Until).Invoke(this.RemoveEffect);
            }
        }

        public void ApplyEffect(GameObject subject) {
            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.AddSpellpowerBonus(_relicEffectData.Description, RPNEvaluator.RPNEvaluator.Evaluate(gainAmount, new()));
        }

        public void RemoveEffect(GameObject subject) {
            var player = Object.FindAnyObjectByType<PlayerInstance>();
            player.RemoveSpellpowerBonus(_relicEffectData.Description);
        }
    }
}