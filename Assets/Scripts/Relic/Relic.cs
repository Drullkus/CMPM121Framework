using System;

namespace Relic {
    public class Relic {
        private RelicData prototype;
        private Action effectOnTrigger;

        // example of eventSubscriber is `event => EventBus.OnKill += event`
        public Relic(RelicData prototype, Action<Action> eventSubscriber) {
            this.prototype = prototype;
            effectOnTrigger = RelicManager.Instance.GetEffect(prototype.Effect.Type);
            eventSubscriber(effectOnTrigger); // Pass our triggerable effect for consumption
        }

        private void Dispatch() {
            effectOnTrigger?.Invoke();
        }
    }
}