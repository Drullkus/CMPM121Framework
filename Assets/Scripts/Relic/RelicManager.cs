using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

namespace Relic {
    public class RelicManager {
        private static readonly Dictionary<string, RelicData> RelicRegistry = new();
        private static readonly Dictionary<string, Action<RelicTriggerData, Action<GameObject>>> RelicTriggerRegistry = new();
        private static readonly Dictionary<string, Func<RelicEffectData, RelicEffect>> RelicEffectRegistry = new();
        private static readonly Dictionary<string, Action<Action<GameObject>>> RelicEventRegistry = new();

        private static RelicManager _theInstance;
        public static RelicManager Instance {
            get {
                if (_theInstance != null) return _theInstance;

                _theInstance = new RelicManager();
                _theInstance.InitializeTypes();
                _theInstance.LoadRelics();
                EventBus.Instance.GameStarted += _theInstance.LoadRelics;
                EventBus.Instance.OnPlayerDeath += _theInstance.UnloadRelics;
                
                return _theInstance;
            }
        }

        private void InitializeTypes() {
            RelicTriggerRegistry.Add("take-damage", (_, gameObjectEffect) => EventBus.Instance.OnTakeHit += gameObjectEffect);
            RelicTriggerRegistry.Add("stand-still", (data, gameObjectEffect) => new StandStill(data, gameObjectEffect));
            RelicTriggerRegistry.Add("on-kill", (_, gameObjectEffect) => EventBus.Instance.OnKill += () => gameObjectEffect.Invoke(GetPlayer()));
            RelicEffectRegistry.Add("gain-mana", data => new GainManaEffect(data));
            RelicEffectRegistry.Add("gain-spellpower", data => new GainSpellpowerEffect(data));

            RelicEventRegistry.Add("move", a => EventBus.Instance.MovementStarted += a);
            
            // Custom
            RelicTriggerRegistry.Add("cast-spell", (_, gameObjectEffect) => EventBus.Instance.OnCastSpell += gameObjectEffect);
            RelicTriggerRegistry.Add("new-wave", (_, gameObjectEffect) => EventBus.Instance.OnCountdownStarted += () => gameObjectEffect.Invoke(GetPlayer()));
            RelicEffectRegistry.Add("damage-nearest", data => new EffectDamageNearest(data));
            RelicEffectRegistry.Add("gain-health", data => new EffectGainHealth(data));
            RelicEffectRegistry.Add("next-spells-free", data => new EffectNextSpellsFree(data));
        }

        private GameObject GetPlayer() {
            return UnityEngine.Object.FindAnyObjectByType<PlayerInstance>().gameObject;
        }

        private void LoadRelics() {
            AssetManager.Instance.Deserialize<List<RelicData>>("relics", PutRelics);
        }

        private void PutRelics(List<RelicData> relicDatas) {
            UnloadRelics();
            Debug.Log($"Loaded {relicDatas.Count} Relics: {string.Join(", ", relicDatas.Select(d => d.Name))}");
            foreach (var relicData in relicDatas) {
                RelicRegistry[relicData.Name] = relicData;
            }
        }
        
        private void UnloadRelics() {
            RelicRegistry.Clear();
        }

        public List<RelicData> GetRandomRelicOptions(HashSet<string> alreadyOwned, int rolls = 3) {
            var listRelics = new List<RelicData>(RelicRegistry.Values).Where(r => !alreadyOwned.Contains(r.Name));

            return listRelics.OrderBy(_ => Random.value).ToList()
                .Take(rolls)
                .ToList();
        }

        public Action<RelicTriggerData, Action<GameObject>> GetTrigger(string triggerName) {
            return RelicTriggerRegistry[triggerName];
        }

        public Func<RelicEffectData, RelicEffect> GetEffect(string triggerName) {
            return RelicEffectRegistry[triggerName];
        }

        public Action<Action<GameObject>> GetEvent(string eventName) {
            return RelicEventRegistry[eventName];
        }

        public RelicData GetRelicData(string relicName) {
            return RelicRegistry[relicName];
        }
    }
}
