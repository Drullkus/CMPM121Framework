namespace Relic {
    public class Relic {
        public Relic(RelicData prototype) {
            var effectOnTrigger1 = RelicManager.Instance.GetEffect(prototype.Effect.Type)(prototype.Effect);
            var triggerConstructor = RelicManager.Instance.GetTrigger(prototype.Trigger.Type);
            triggerConstructor(prototype.Trigger, effectOnTrigger1.ApplyEffect);
        }
    }
}