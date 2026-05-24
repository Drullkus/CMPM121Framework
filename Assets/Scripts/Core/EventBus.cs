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
    
    public event Action<GameObject> MovementStarted;
    public void DoMovementStarted(GameObject unit) {
        MovementStarted?.Invoke(unit);
    }
    
    public event Action<GameObject> MovementStopped;
    public void DoMovementStopped(GameObject unit) {
        MovementStopped?.Invoke(unit);
    }
    
    public event Action<GameObject> OnTakeHit;
    public void DoOnTakeHit(GameObject dying) {
        OnTakeHit?.Invoke(dying);
    }
    
    public event Action<GameObject> OnKill;
    public void DoOnKill(GameObject killer) {
        OnKill?.Invoke(killer);
    }
    
    public event Action<GameObject> OnDeath;
    public void DoOnDeath(GameObject dying) {
        OnDeath?.Invoke(dying);
    }
    
    public event Action GameStarted;
    public void DoGameStarted() {
        GameStarted?.Invoke();
    }
    
    public event Action GameStopped;
    public void DoGameStopped() {
        GameStopped?.Invoke();
    }

}
