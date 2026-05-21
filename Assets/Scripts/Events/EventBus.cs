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

    public event Action<Vector3, Damage, IHittable> OnDamage;
    
    public void DoDamage(Vector3 where, Damage dmg, IHittable target) {
        OnDamage?.Invoke(where, dmg, target);
    }

	public event Action<UIScreen> OnUIScreenRegistered;
	public void RegisterUIScreen(UIScreen uiScreen) {
		OnUIScreenRegistered?.Invoke(uiScreen);
	}

	public event Action<Level> OnDifficultyChosen;
	public void ChooseDifficulty(Level level) {
		OnDifficultyChosen?.Invoke(level);
	}

	public event Action OnUIScreenClosed;
	public void CloseUIScreen() {
		OnUIScreenClosed?.Invoke();
	}

	public event Action<int> OnWaveStarted;
	public void StartWave(int totalEnemyCount) {
		OnWaveStarted?.Invoke(totalEnemyCount);
	}

	public event Action OnWaveEnded;
	public void EndWave() {
		OnWaveEnded?.Invoke();
	}

	public event Action<int, Spawn> OnSpawnSchedulingRequested;
	public void RequestSpawnScheduling(int waveIndex, Spawn spawn) {
		OnSpawnSchedulingRequested?.Invoke(waveIndex, spawn);
	}

	public event Action OnPlayerDeath;
	public void InvokePlayerDeath() {
		OnPlayerDeath?.Invoke();
	}

	public event Action OnPlayerShoot;
	public void InvokePlayerShoot() {
		OnPlayerShoot?.Invoke();
	}

	public event Action OnEnemyHit;
	public void InvokeEnemyHit() {
		OnEnemyHit?.Invoke();
	}

	public event Action OnEnemyDefeated;
	public void InvokeEnemyDefeated() {
		OnEnemyDefeated?.Invoke();
	}

	public event Action OnAllEnemiesDefeated;
	public void InvokeAllEnemiesDefeated() {
		OnAllEnemiesDefeated?.Invoke();
	}

	public void InvokeDebugMessage(string s) {
		Debug.Log(s);
	}

}
