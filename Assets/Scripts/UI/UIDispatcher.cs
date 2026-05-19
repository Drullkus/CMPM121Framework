using System;
using UnityEngine;

public class UIDispatcher {
	
	public enum UIState {
		REWARD,
		WAVE,
	}

	private UIState _state;

	private static UIDispatcher _instance;
	public static UIDispatcher Instance {
		get {
			_instance ??= new();
			return _instance;
		}
	}

	private UIDispatcher() {
		EventBus.Instance.OnUIGameObjectRegistered += RegisterUIObject;
	}

	// TODO
	private void RegisterUIObject(Action show, Action hide, UIState state) { }

	// TODO
	private void ChangeState(UIState newState) { }

}

