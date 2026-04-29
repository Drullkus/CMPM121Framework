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

    public int GetCountInWave(int wave)
    {
        return RPNEvaluator.RPNEvaluator.Evaluate(this.Count, new() { ["wave"] = wave });
    }

    public int GetDelayInWave(int wave)
    {
        return RPNEvaluator.RPNEvaluator.Evaluate(this.Delay, new() { ["wave"] = wave });
    }

    public int GetHpInWave(int baseHp, int wave)
    {
        return RPNEvaluator.RPNEvaluator.Evaluate(this.Hp, new() { ["base"] = baseHp, ["wave"] = wave });
    }

    public int GetSpeedInWave(int baseHp, int wave)
    {
        return RPNEvaluator.RPNEvaluator.Evaluate(this.Speed, new() { ["base"] = baseHp, ["wave"] = wave });
    }

    public int GetDamageInWave(int baseHp, int wave)
    {
        return RPNEvaluator.RPNEvaluator.Evaluate(this.Damage, new() { ["base"] = baseHp, ["wave"] = wave });
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