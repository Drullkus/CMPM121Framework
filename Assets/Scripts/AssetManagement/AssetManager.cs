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

	void Awake() {
		if(Instance != null) { return; }

		_instance = this;
	}

	// TODO
	public void LoadSprites(string spritesheetPath, Action<Sprite[]> onLoad) { }

	// TODO
	public void LoadJson(string jsonPath, Action<string> onLoad) { }

}

