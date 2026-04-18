using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class Level
{
    [JsonProperty("name")]
    public string Name { get; }

    [JsonProperty("waves", DefaultValueHandling = DefaultValueHandling.Populate)]
    public int Waves { get; set; } = 0; // 0 means endless

    [JsonProperty("spawns")]
    public List<Spawn> Spawns { get; set; }
}