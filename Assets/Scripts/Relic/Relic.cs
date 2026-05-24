using System;

namespace Relic {
    public class Relic {
        private RelicData prototype;
        private Action effectOnTrigger;

        // example of eventSubscriber is `event => EventBus.OnKill += event`
        public Relic(RelicData prototype) {
            this.prototype = prototype;
            effectOnTrigger = RelicManager.Instance.GetEffect(prototype.Effect.Type);
            RelicManager.Instance.GetTrigger(prototype.Trigger.Type)(Dispatch); // Pass our triggerable effect for consumption by trigger. Should subscribe the effect to listen to a publisher
        }

        private void Dispatch() {
            effectOnTrigger?.Invoke();
        }
    }
}