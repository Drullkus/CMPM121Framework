using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;

[System.Serializable]
public class Spawn
{
    [JsonProperty("enemy")]
    public string Enemy { get; set; }

    [JsonProperty("count")]
    public string Count { get; set; }

    [DefaultValue("base")]
    [JsonProperty("hp", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Hp { get; set; }

    [DefaultValue("base")]
    [JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Speed { get; set; }

    [DefaultValue("base")]
    [JsonProperty("damage", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Damage { get; set; }

    [DefaultValue("2")]
    [JsonProperty("delay", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Delay { get; set; }

    [DefaultValue(new int[] { 1 })]
    [JsonProperty("sequence", DefaultValueHandling = DefaultValueHandling.Populate)]
    public int[] Sequence { get; set; }

    [DefaultValue("random")]
    [JsonProperty("location", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Location { get; set; }

    public void CalculateForWave(int wave, out int count, out int delay)
    {
        Dictionary<string, int> variables = new() { ["wave"] = wave };

        count = RPNEvaluator.RPNEvaluator.Evaluate(this.Count, variables);
        delay = RPNEvaluator.RPNEvaluator.Evaluate(this.Delay, variables);
    }

    public void CalculateForNewSpawn(EnemyStats enemy, int wave, out int hp, out int speed, out int damage)
    {
        hp = RPNEvaluator.RPNEvaluator.Evaluate(this.Hp, new() { ["base"] = enemy.HP, ["wave"] = wave });
        speed = RPNEvaluator.RPNEvaluator.Evaluate(this.Speed, new() { ["base"] = enemy.Speed, ["wave"] = wave });
        damage = RPNEvaluator.RPNEvaluator.Evaluate(this.Damage, new() { ["base"] = enemy.Damage, ["wave"] = wave });
    }

    public IEnumerable<int> GetSpawnBatches()
    {
        while (true)
        {
            foreach (int batch in Sequence)
            {
                yield return batch;
            }
        }
    }

}