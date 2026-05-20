using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class WaveStat {

	[JsonProperty("evaluationFormat")]
	private string _evaluationFormat;

	[JsonProperty("displayFormat")]
	private string _displayFormat;

	public float Evaluate(
		Dictionary<string, float> baseStats,
		Dictionary<string, WaveStat> derivedStats
	) {
		string evaluableString = _evaluationFormat;

		foreach(KeyValuePair<string, WaveStat> derivedStatKeyValue in derivedStats) {
			string key = derivedStatKeyValue.Key;
			
			if(!evaluableString.Contains(key)) { continue; }

			float value = derivedStatKeyValue.Value.Evaluate(baseStats, derivedStats);

			evaluableString = evaluableString.Replace(key, value.ToString());
		}

		return RPNEvaluator.RPNEvaluator.Evaluatef(evaluableString, baseStats);
	}

	// TODO
	public string Format() { return ""; }

}

public class WaveStatTracker {

	private static WaveStatTracker _instance;
	public static WaveStatTracker Instance {
		get {
			_instance ??= new();
			return _instance;
		}
	}

	private WaveStatTracker() {
		EventBus.Instance.OnWaveStart += () => { _baseStats["waveDuration"] = Time.time; };
		EventBus.Instance.OnWaveEnd += () => { _baseStats["waveDuration"] = Time.time - _baseStats["waveDuration"]; };

		EventBus.Instance.OnEnemyHit += () => { _baseStats["hitCount"]++; };
		EventBus.Instance.OnPlayerShoot += () => { _baseStats["shotCount"]++; };

		AssetManager.Instance.LoadJson("stats", (loadedJson) => {
			_derivedStats = JsonConvert.DeserializeObject<Dictionary<string, WaveStat>>(loadedJson);
		});
	}

	private Dictionary<string, float> _baseStats = new() {
		[ "hitCount" ] = 0.0f,
		[ "shotCount" ] = 0.0f,
		[ "waveDuration" ] = 0.0f,
	};

	private Dictionary<string, WaveStat> _derivedStats;

	// TODO
	public List<string> GetRandomFormattedStats(int max) { return new(); }

}
