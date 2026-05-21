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
    
    public event Action<Unit> MovementStarted;
    public void DoMovementStarted(Unit unit) {
        MovementStarted?.Invoke(unit);
    }
    
    public event Action<Unit> MovementStopped;
    public void DoMovementStopped(Unit unit) {
        MovementStopped?.Invoke(unit);
    }
    
    public event Action<Unit, Unit> OnKill;
    public void DoOnKill(Unit killer, Unit victim) {
        OnKill?.Invoke(killer, victim);
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
