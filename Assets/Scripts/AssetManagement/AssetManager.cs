using System;
using UnityEngine;

public class AssetManager : MonoBehaviour {

	public static AssetManager Instance;

	void Awake() {
		if(Instance != null) { Destroy(gameObject); return; }

		Instance = this;
	}

	// TODO
	public void LoadSprites(string spritesheetPath, Action<Sprite[]> onLoad) { }

	// TODO
	public void LoadJson(string jsonPath, Action<string> onLoad) { }

}

