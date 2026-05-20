using System.Collections.Generic;

public class WaveStat {

	// TODO
	public float Evaluate(Dictionary<string, WaveStat> variables) { return 0.0f; }

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

	// TODO
	private WaveStatTracker() { }

	// TODO
	public Dictionary<string, WaveStat> statValues = new(){ };

	// TODO
	public List<string> GetRandomFormattedStats(int max) { return new(); }

}
