using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInstance :
	MonoBehaviour, IHittable
{

	private HP _hp;

    private PlayerClassData _data;

    public int speed;

    void Start() {
        _data = new();
        
        _hp.OnExpended += Die;

        gameObject.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrievePlayerSprite(_data.sprite);
    }

	public void Hit(Damage damage) {
		_hp.TakeDamage(damage);
	}

    void Die() {
		EventBus.Instance.InvokePlayerDeath();
    }

}
