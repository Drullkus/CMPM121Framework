using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class Spell {

    public float last_cast;
    public SpellCaster owner;
    public Team team;

    public Spell(SpellCaster owner) {
        this.owner = owner;
    }

    public string GetName() {
        return "Bolt";
    }

    public int GetManaCost() {
        return 10;
    }

    public int GetDamage() {
        return 100;
    }

    public float GetCooldown() {
        return 0.75f;
    }

    public virtual int GetIcon() {
        return 0;
    }

    public bool IsReady() {
        return (last_cast + GetCooldown() < Time.time);
    }

	// TODO
    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Team team) {
        this.team = team;
        yield return new WaitForEndOfFrame();
    }

    void OnHit(Team otherTeam, IHittable other, Vector3 impact) {
        if (otherTeam != team) {
            other.Hit(new Damage(GetDamage(), Damage.Type.ARCANE));
        }
    }

}
