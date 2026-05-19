using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIState {
	REWARD,
	WAVE,
}

public class UIDispatcher {

	private UIState _state;

	private Dictionary<UIState, UIObject> _objectMap = new();

	private static UIDispatcher _instance;
	public static UIDispatcher Instance {
		get {
			_instance ??= new();
			return _instance;
		}
	}

	private UIDispatcher() {
		EventBus.Instance.OnUIGameObjectRegistered += RegisterUIObject;
		EventBus.Instance.OnUIStateChanged += ChangeState;
	}

	private void RegisterUIObject(UIObject uiObject, UIState state) {
		if(!_objectMap.TryAdd(state, uiObject)) {
			Debug.LogWarning("Tried to associate 2 UIObjects with the same UIState.");
		}
	}

	private void ChangeState(UIState newState) {
		if(_state == newState) { return; }

		Action hideAction = () => {};
		Action showAction = () => {};

		if(_state != UIState.WAVE) {
			hideAction = _objectMap.TryGetValue(_state, out UIObject oldUIObject) ?
				oldUIObject.Hide :
				() => { Debug.LogError("Tried to change UIState to an invalid value!"); };
		}

		if(newState != UIState.WAVE) {
			showAction = _objectMap.TryGetValue(newState, out UIObject newUIObject) ?
				newUIObject.Show :
				() => { Debug.LogError("Tried to change UIState from an invalid value!"); };
		}

		hideAction.Invoke();
		showAction.Invoke();

		_state = newState;
	}

}

