using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Player {
    public class PlayerClassData
    {
        [JsonProperty("sprite")] public int sprite { get; set; } = 0;

        [DefaultValue("100")]
        [JsonProperty("health", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string health { get; set; } = "100";

        [DefaultValue("100")]
        [JsonProperty("mana", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string mana { get; set; } = "100";

        [DefaultValue("10")]
        [JsonProperty("mana_regeneration", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string mana_regeneration { get; set; } = "10";

        [DefaultValue("5")]
        [JsonProperty("spellpower", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string spellpower { get; set; } = "5";

        [DefaultValue("5")]
        [JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string speed { get; set; } = "5";

        public void CalculatePlayerStatsForWave(int wave, out int health, out int mana, out int mana_regeneration, out int spellpower, out int speed) {
            Dictionary<string, int> variables = new() { ["wave"] = wave };
            
            health = RPNEvaluator.RPNEvaluator.Evaluate(this.health, variables);
            mana = RPNEvaluator.RPNEvaluator.Evaluate(this.mana, variables);
            mana_regeneration = RPNEvaluator.RPNEvaluator.Evaluate(this.mana_regeneration, variables);
            spellpower = RPNEvaluator.RPNEvaluator.Evaluate(this.spellpower, variables);
            speed = RPNEvaluator.RPNEvaluator.Evaluate(this.speed, variables);
        }
    }
}