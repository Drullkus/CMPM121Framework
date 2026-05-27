using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class PlayerClassManager {

	private static Dictionary<string, PlayerClassData> _classData;

	private static void HandleJsonLoaded(string json, Action<Dictionary<string, PlayerClassData>> onDeserialized) {
		_classData = JsonConvert.DeserializeObject<Dictionary<string, PlayerClassData>>(json);
		onDeserialized.Invoke(_classData);
	}

	public static void GetClasses(Action<Dictionary<string, PlayerClassData>> onGotten) {
		if(_classData != null) {
			onGotten(_classData);
			return;
		}

		AssetManager.Instance.LoadJson("classes", (json) => { HandleJsonLoaded(json, onGotten); });
	}

}
