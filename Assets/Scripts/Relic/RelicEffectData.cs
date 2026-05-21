using System.ComponentModel;
using Newtonsoft.Json;

namespace Relic {
    public class RelicEffectData {
        [DefaultValue("missing description")]
        [JsonProperty("description", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Type { get; set; }
        
        [DefaultValue("0")]
        [JsonProperty("amount", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Amount { get; set; }
        
        [DefaultValue("")]
        [JsonProperty("until", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Until { get; set; }
    }
}