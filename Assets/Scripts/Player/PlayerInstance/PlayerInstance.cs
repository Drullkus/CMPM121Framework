using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInstance :
	MonoBehaviour, IHittable
{

	private HP _hp;

	private Vector2 _movement;

    private PlayerClassData _data;

    public int speed;

    void Start() {
		_hp = new(100);
	}

	private void Attack() {
		EventBus.Instance.InvokePlayerShoot();

		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
	}

	public void OnMove(InputAction.CallbackContext context) {
		_movement = context.ReadValue<Vector2>();
	}

	private void Move(Vector2 direction) {
		List<RaycastHit2D> hits = new List<RaycastHit2D>();
		int collisionCount = GetComponent<Rigidbody2D>().Cast(direction, hits, direction.magnitude * 2.0f);

		if(collisionCount > 0) { return; }

		transform.Translate(direction);
	}

	private void FixedUpdate() {
		Move(new Vector2(_movement.x, 0.0f) * Time.fixedDeltaTime);
		Move(new Vector2(0.0f, _movement.y) * Time.fixedDeltaTime);
	}

	public void Hit(Damage damage) {
		_hp.TakeDamage(damage);
	}

    void Die() {
		EventBus.Instance.InvokePlayerDeath();
    }

}
