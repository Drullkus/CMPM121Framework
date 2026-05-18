using System;
using UnityEngine;

public class AssetManager {

	private static AssetManager _instance;
	public static AssetManager Instance {
		get {
			_instance ??= new AssetManager();
			return _instance;
		}
	}

	public void LoadSprites(string spritesheetPath, Action<Sprite[]> onLoad) {
		onLoad.Invoke(Resources.LoadAll<Sprite>(spritesheetPath));
	}

	public void LoadJson(string jsonPath, Action<string> onLoad) {
		onLoad.Invoke(Resources.Load<TextAsset>(jsonPath).text);
	}

}

