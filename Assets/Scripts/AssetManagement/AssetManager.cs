using Newtonsoft.Json;
using System;
using UnityEngine;

public class AssetManager {

	public GameObject projectilePrefab;

	private static AssetManager _instance;
	public static AssetManager Instance {
		get {
			if(_instance == null) {
				_instance = new AssetManager();

				_instance.LoadPrefab("projectile", (loadedPrefab) => {
					_instance.projectilePrefab = loadedPrefab;
				});
			}

			return _instance;
		}
	}

	public void LoadSprites(string spritesheetPath, Action<Sprite[]> onLoad) {
		onLoad.Invoke(Resources.LoadAll<Sprite>(spritesheetPath));
	}

	public void LoadJson(string jsonPath, Action<string> onLoad) {
		onLoad.Invoke(Resources.Load<TextAsset>(jsonPath).text);
	}

	public void Deserialize<T>(string jsonPath, Action<T> onLoad) {
		onLoad.Invoke(JsonConvert.DeserializeObject<T>(Resources.Load<TextAsset>(jsonPath).text));
	}

	public void LoadPrefab(string prefabPath, Action<GameObject> onLoad) {
		onLoad.Invoke(Resources.Load<GameObject>(prefabPath));
	}

}

