using UnityEngine;

namespace Relic.RelicEffect {
    public class GainManaEffect : RelicEffect {
        private readonly RelicEffectData _relicEffectData;

        public GainManaEffect(RelicEffectData relicEffectData) {
            _relicEffectData = relicEffectData;
        }
        
        public void ApplyEffect(GameObject subject) {
            throw new System.NotImplementedException();
        }
    }
}