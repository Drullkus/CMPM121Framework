using System;
using System.Collections.Generic;
using UnityEngine;

public enum UIState {
	REWARD,
	WAVE,
}

public class UIObject {

	private Action _show;
	private Action _hide;

	public UIObject(Action show, Action hide) {
		_show = show;
		_hide = hide;
	}

	public void Show() { _show.Invoke(); }
	public void Hide() { _hide.Invoke(); }

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

		if(!_objectMap.TryGetValue(_state, out UIObject oldUIObject)) {
			Debug.LogError("Tried to change UIState from an invalid value!");
			return;
		}

		if(!_objectMap.TryGetValue(newState, out UIObject newUIObject)) {
			Debug.LogError("Tried to change UIState to an invalid value!");
			return;
		}

		oldUIObject.Hide();
		newUIObject.Show();
	}

}

