using UnityEngine;

public class DamageNumbers : MonoBehaviour {

    public GameObject DamageNumber;

    void Start() {
        EventBus.Instance.OnDamage += OnDamage;
    }

    public void OnDestroy() {
        EventBus.Instance.OnDamage -= OnDamage;
    }

    void OnDamage(Vector3 where, Damage dmg, IHittable target) {
        var damageNumber = Instantiate(DamageNumber, where, Quaternion.identity);
        Vector3 dmg_pos = where + new Vector3(0, 0, -2);
        damageNumber.GetComponent<AnimateDamage>().Setup(dmg.amount.ToString(), dmg_pos, dmg_pos + new Vector3(0, 3, 0), 10, 2, Color.magenta, Color.white, 1.5f);
    }

}
