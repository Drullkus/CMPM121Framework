using System.ComponentModel;
using Newtonsoft.Json;

namespace Player
{
    public class PlayerClassData
    {
        [JsonProperty("sprite")]
        public int sprite;
        [DefaultValue("100")]
        [JsonProperty("health", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string health;
        [DefaultValue("100")]
        [JsonProperty("mana", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string mana;
        [DefaultValue("10")]
        [JsonProperty("mana_regeneraton", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string mana_regeneraton;
        [DefaultValue("5")]
        [JsonProperty("spellpower", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string spellpower;
        [DefaultValue("5")]
        [JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string speed;
    }
}