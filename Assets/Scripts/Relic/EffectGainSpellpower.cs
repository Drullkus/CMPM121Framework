using UnityEngine;

namespace Relic.RelicEffect {
    public class GainSpellpowerEffect : RelicEffect {
        private readonly RelicEffectData _relicEffectData;

        public GainSpellpowerEffect(RelicEffectData relicEffectData) {
            _relicEffectData = relicEffectData;
        }

        public void ApplyEffect(GameObject subject) {
            throw new System.NotImplementedException();
        }
    }
}