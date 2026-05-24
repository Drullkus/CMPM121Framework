using System;
using UnityEngine;

namespace Relic {
    public class Relic {
        private Action<GameObject> effectOnTrigger;

        public Relic(RelicData prototype) {
            effectOnTrigger = RelicManager.Instance.GetEffect(prototype.Effect.Type);
            var triggerConstructor = RelicManager.Instance.GetTrigger(prototype.Trigger.Type);
            triggerConstructor(prototype.Trigger, effectOnTrigger);
        }
    }
}