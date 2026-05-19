using UnityEngine;
using System;

public class EventBus {

    private static EventBus theInstance;
    public static EventBus Instance {
        get {
            theInstance ??= new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;
    
    public void DoDamage(Vector3 where, Damage dmg, Hittable target) {
        OnDamage?.Invoke(where, dmg, target);
    }

	public event Action<Action, Action, UIDispatcher.UIState> OnUIGameObjectRegistered;
	public void RegisterUIGameObject(Action show, Action hide, UIDispatcher.UIState state) {
		OnUIGameObjectRegistered?.Invoke(show, hide, state);
	}

	public event Action<int, Spawn> OnSpawnSchedulingRequested;
	public void RequestSpawnScheduling(int waveIndex, Spawn spawn) {
		OnSpawnSchedulingRequested?.Invoke(waveIndex, spawn);
	}

	public event Action OnAllEnemiesDefeated;
	public void InvokeAllEnemiesDefeated() {
		OnAllEnemiesDefeated?.Invoke();
	}

	public event Action OnWaveEnd;
	public void InvokeWaveEnd() {
		OnWaveEnd?.Invoke();
	}

}
