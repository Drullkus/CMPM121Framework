using Newtonsoft.Json;

[System.Serializable]
public class Spawn
{
    [JsonProperty("enemy")]
    public string Enemy { get; set; }

    [JsonProperty("count")]
    public string Count { get; set; }

    [JsonProperty("hp", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Hp { get; set; } = "base";

    [JsonProperty("speed", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Speed { get; set; } = "base";

    [JsonProperty("damage", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Damage { get; set; } = "base";

    [JsonProperty("delay", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Delay { get; set; } = "2";

    [JsonProperty("sequence", DefaultValueHandling = DefaultValueHandling.Populate)]
    public int[] Sequence { get; set; } = { 1 };

    [JsonProperty("location", DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Location { get; set; } = "random";

    public int GetCountInWave(int wave)
    {
        return RPNEvaluator.RPNEvaluator.Evaluate(this.Count, new() { ["wave"] = wave });
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

}