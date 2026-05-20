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

	public event Action<UIObject> OnUIObjectRegistered;
	public void RegisterUIObject(UIObject uiObject) {
		OnUIObjectRegistered?.Invoke(uiObject);
	}

	public event Action OnWaveStart;
	public void StartWave() {
		OnWaveStart?.Invoke();
	}

	public event Action OnWaveEnd;
	public void EndWave() {
		OnWaveEnd?.Invoke();
	}

	public event Action<int, Spawn> OnSpawnSchedulingRequested;
	public void RequestSpawnScheduling(int waveIndex, Spawn spawn) {
		OnSpawnSchedulingRequested?.Invoke(waveIndex, spawn);
	}

	public event Action OnEnemyDefeated;
	public void InvokeEnemyDefeated() {
		OnEnemyDefeated?.Invoke();
	}

	public event Action OnAllEnemiesDefeated;
	public void InvokeAllEnemiesDefeated() {
		OnAllEnemiesDefeated?.Invoke();
	}

}
