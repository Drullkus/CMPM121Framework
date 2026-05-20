using System.Collections.Generic;

public abstract class WaveStat {

	public abstract float Evaluate();
	public abstract string Format();

}

public class SimpleWaveStat : WaveStat {

	private float _value;
	private string _format;

	public SimpleWaveStat(ref float valueToTrack, string format) {
		_value = valueToTrack;
		_format = format;
	}

	public override float Evaluate() { return _value; }
	public override string Format() { return ""; }

}

public class CompoundWaveStat : WaveStat {

	public override float Evaluate() { return 0.0f; }
	public override string Format() { return ""; }

}

public class WaveStatTracker {

	private static WaveStatTracker _instance;
	public static WaveStatTracker Instance {
		get {
			_instance ??= new();
			return _instance;
		}
	}

	public Dictionary<string, WaveStat> statValues = new(){
		
	};

}
