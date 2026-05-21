using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

namespace Relic {
    public class RelicManager {
        private static readonly Dictionary<string, RelicData> RelicRegistry = new();
        private static readonly Dictionary<string, Action> RelicTriggerRegistry = new();
        private static readonly Dictionary<string, Action> RelicEffectRegistry = new();

        private static RelicManager _theInstance;
        public static RelicManager Instance {
            get {
                if (_theInstance != null) return _theInstance;

                _theInstance = new RelicManager();
                EventBus.Instance.GameStarted += LoadRelics;
                EventBus.Instance.GameStopped += UnloadRelics;
                
                return _theInstance;
            }
        }

        private static void LoadRelics() {
            var relicsJsonAsset = Resources.Load<TextAsset>("relics");
            var relicDatas = JsonConvert.DeserializeObject<List<RelicData>>(relicsJsonAsset.text);
            foreach (var relicData in relicDatas) {
                RelicRegistry[relicData.Name] = relicData;
            }
        }
        
        private static void UnloadRelics() {
            RelicRegistry.Clear();
        }

        public List<RelicData> GetRandomRelics(HashSet<string> alreadyOwned, int rolls = 3) {
            var listRelics = new List<RelicData>(RelicRegistry.Values).Where(r => !alreadyOwned.Contains(r.Name));

            return listRelics.OrderBy(_ => Random.value)
                .Take(rolls)
                .ToList();
        }

        public Action GetTrigger(string triggerName) {
            return RelicTriggerRegistry[triggerName];
        }

        public Action GetEffect(string triggerName) {
            return RelicEffectRegistry[triggerName];
        }
    }
}
