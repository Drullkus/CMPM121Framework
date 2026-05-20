using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDispatcher : MonoBehaviour {

	private UIState _state;

	private Dictionary<UIState, UIScreen> _objectMap = new();

	public static UIDispatcher Instance;

	private void Awake() {
		if(Instance != null) {
			if(Instance != this) {
				Destroy(this);
			}

			return;
		}

		EventBus.Instance.OnUIScreenRegistered += RegisterUIScreen;
		EventBus.Instance.OnUIScreenClosed += () => { ChangeState(UIState.WAVE); };
		EventBus.Instance.OnWaveEnded += () => { ChangeState(UIState.REWARD); };

		Instance = this;
	}

	private void RegisterUIScreen(UIScreen uiScreen) {
		if(!_objectMap.TryAdd(uiScreen.state, uiScreen)) {
			Debug.LogWarning("Tried to associate 2 UIObjects with the same UIState.");
			return;
		}
		
		// this is pretty sloppy, we should implement a
		// default state type thing
		if(uiScreen.state == UIState.LEVEL_SELECT) {
			uiScreen.Show();
		}
	}

	private void ChangeState(UIState newState) {
		if(_state == newState) { return; }

		Action hideAction = () => {};
		Action showAction = () => {};

		if(_state != UIState.WAVE) {
			hideAction = _objectMap.TryGetValue(_state, out UIScreen oldUIScreen) ?
				oldUIScreen.Hide :
				() => { Debug.LogError("Tried to change UIState to an invalid value!"); };
		}

		if(newState != UIState.WAVE) {
			showAction = _objectMap.TryGetValue(newState, out UIScreen newUIScreen) ?
				newUIScreen.Show :
				() => { Debug.LogError("Tried to change UIState from an invalid value!"); };
		}

		hideAction.Invoke();
		showAction.Invoke();

		_state = newState;
	}

}

