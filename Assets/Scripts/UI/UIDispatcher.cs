using System;
using System.Collections.Generic;
using UnityEngine;

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
	
	public enum UIState {
		REWARD,
		WAVE,
	}

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
	}

	private void RegisterUIObject(UIObject uiObject, UIState state) {
		if(!_objectMap.TryAdd(state, uiObject)) {
			Debug.LogWarning("Tried to associate 2 UIObjects with the same UIState.");
		}
	}

	// TODO
	private void ChangeState(UIState newState) { }

}

