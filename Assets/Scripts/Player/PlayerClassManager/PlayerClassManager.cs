using Newtonsoft.Json;
using System.Collections.Generic;

public class PlayerClassManager {

	private Dictionary<PlayerClassData> _classData;

	private static void HandleJsonLoaded(string json, Action<Dictionary<PlayerClassData>> onDeserialized) {
		_classData = onDeserialized.Invoke(JsonConvert.DeserializeObject<List<PlayerClassData>>(json));
		onDeserialized.Invoke(_classData);
	}

	public static void GetClasses(Action<PlayerClassData> onGotten) {
		if(_classData != null) {
			onGotten(_classData);
			return;
		}

		AssetManager.LoadJson("classes", (json) => { HandleJsonLoaded(json, onGotten); });
	}

}
