using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Relic.RelicEffect;
using Relic.RelicTrigger;
using Random = UnityEngine.Random;

namespace Relic {
    public class RelicManager {
        private static readonly Dictionary<string, RelicData> RelicRegistry = new();
        private static readonly Dictionary<string, Action<RelicTriggerData, Action<GameObject>>> RelicTriggerRegistry = new();
        private static readonly Dictionary<string, Func<RelicEffectData, RelicEffect.RelicEffect>> RelicEffectRegistry = new();

        private static RelicManager _theInstance;
        public static RelicManager Instance {
            get {
                if (_theInstance != null) return _theInstance;

                _theInstance = new RelicManager();
                _theInstance.InitializeTypes();
                EventBus.Instance.GameStarted += _theInstance.LoadRelics;
                EventBus.Instance.OnPlayerDeath += _theInstance.UnloadRelics;
                
                return _theInstance;
            }
        }

        private void InitializeTypes() {
            RelicTriggerRegistry.Add("take-damage", (_, gameObjectEffect) => EventBus.Instance.OnTakeHit += gameObjectEffect);
            RelicTriggerRegistry.Add("stand-still", (data, gameObjectEffect) => new StandStill(data, gameObjectEffect));
            RelicTriggerRegistry.Add("on-kill", (_, gameObjectEffect) => EventBus.Instance.OnKill += gameObjectEffect);

            RelicEffectRegistry.Add("gain-mana", data => new GainManaEffect(data));
            RelicEffectRegistry.Add("gain-spellpower", data => new GainSpellpowerEffect(data));
        }

        private void LoadRelics() {
            var relicsJsonAsset = Resources.Load<TextAsset>("relics");
            var relicDatas = JsonConvert.DeserializeObject<List<RelicData>>(relicsJsonAsset.text);
            foreach (var relicData in relicDatas) {
                RelicRegistry[relicData.Name] = relicData;
            }
            // Debug.Log($"Loaded {relicDatas.Count} Relics: {string.Join(", ", relicDatas.Select(d => d.Name))}");
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

        public Func<RelicEffectData, RelicEffect.RelicEffect> GetEffect(string triggerName) {
            return RelicEffectRegistry[triggerName];
        }

        public Relic InstantiateRelic(string relicName) {
            return new Relic(RelicRegistry[relicName]);
        }
    }
}
