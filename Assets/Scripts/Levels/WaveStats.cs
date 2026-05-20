using UnityEngine;
using System.Collections.Generic;

public class WaveStat {

	// TODO
	public float Evaluate(
		Dictionary<string, float> baseStats,
		Dictionary<string, WaveStat> derivedStats
	) { return 0.0f; }

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
	}

	private Dictionary<string, float> _baseStats = new() {
		[ "hitCount" ] = 0.0f,
		[ "shotCount" ] = 0.0f,
		[ "waveDuration" ] = 0.0f,
	};

	// TODO
	private Dictionary<string, WaveStat> _derivedStats = new(){ };

	// TODO
	public List<string> GetRandomFormattedStats(int max) { return new(); }

}
