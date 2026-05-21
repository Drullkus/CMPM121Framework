using System.ComponentModel;
using Newtonsoft.Json;

namespace Relic {
    public class RelicData {
        [DefaultValue("")]
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Name { get; set; }
        
        [DefaultValue(0)]
        [JsonProperty("sprite", DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Sprite { get; set; }
        
        [JsonProperty("trigger")]
        public RelicTriggerData Trigger { get; set; }
        
        [JsonProperty("effect")]
        public RelicEffectData Effect { get; set; }
    }
}