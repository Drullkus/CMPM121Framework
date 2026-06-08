using UnityEngine;
using System;

public class EventBus {

    private static EventBus _instance;
    public static EventBus Instance {
        get {
            _instance ??= new EventBus();
            return _instance;
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

	public event Action OnWaveRequested;
	public void RequestNextWave() {
		OnWaveRequested?.Invoke();
	}

	public event Action OnCountdownStarted;
	public void StartCountdown() {
		OnCountdownStarted?.Invoke();
	}

	public event Action<int, int> OnWaveStarted;
	public void StartWave(int waveIndex, int totalEnemyCount) {
		OnWaveStarted?.Invoke(waveIndex, totalEnemyCount);
	}

	public event Action OnWaveEnded;
	public void EndWave() {
		OnWaveEnded?.Invoke();
	}

	public event Action<int, BatchSpawnData> OnSpawnSchedulingRequested;
	public void RequestSpawnScheduling(int waveIndex, BatchSpawnData batchSpawnData) {
		OnSpawnSchedulingRequested?.Invoke(waveIndex, batchSpawnData);
	}

	public event Action OnPlayerDeath;
	public void InvokePlayerDeath() {
		OnPlayerDeath?.Invoke();
	}

	public event Action OnPlayerShoot;
	public void InvokePlayerShoot() {
		OnPlayerShoot?.Invoke();
	}

	public event Action<GameObject, float, float> OnRecoil;
	public void InvokeRecoil(GameObject source, float timer, float force) {
		OnRecoil?.Invoke(source, timer, force);
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

	public event Action OnLastWaveEnded;
	public void EndLastWave() {
		OnLastWaveEnded?.Invoke();
	}

	public event Action OnRestartRequested;
	public void RequestRestart() {
		OnRestartRequested?.Invoke();

		// this is a little sloppy. instead of unsubscribing from
		// all the events we've subscribed to in the scripts that
		// subscribed to those events on reset, we just clear the
		// old EventManager instance.
		_instance = new();
	}

	public void InvokeDebugMessage(string s) {
		Debug.Log(s);
	}
	
	public event Action GameStarted;
	public void DoGameStarted() {
		GameStarted?.Invoke();
	}
    
	public event Action GameStopped;
	public void DoGameStopped() {
		GameStopped?.Invoke();
	}
	
	public event Action<GameObject> OnTakeHit;
	public void DoOnTakeHit(GameObject dying) {
		OnTakeHit?.Invoke(dying);
	}
	
	public event Action<GameObject> MovementStarted;
	public void DoMovementStarted(GameObject unit) {
		MovementStarted?.Invoke(unit);
	}
    
	public event Action<GameObject> MovementStopped;
	public void DoMovementStopped(GameObject unit) {
		MovementStopped?.Invoke(unit);
	}
    
	public event Action<GameObject> OnKill;
	public void DoOnKill(GameObject killer) {
		OnKill?.Invoke(killer);
	}
    
	public event Action<GameObject> OnDeath;
	public void DoOnDeath(GameObject dying) {
		OnDeath?.Invoke(dying);
	}
    
	public event Action<GameObject> OnCastSpell;
	public void DoOnCastSpell(GameObject player) {
		OnCastSpell?.Invoke(player);
	}
    
	public event Action<GameObject> OnNewWave;
	public void DoOnNewWave(GameObject player) {
		OnNewWave?.Invoke(player);
	}

}
