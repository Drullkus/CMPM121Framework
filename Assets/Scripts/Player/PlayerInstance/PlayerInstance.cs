using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInstance :
	MonoBehaviour, IHittable
{

	private Vector2 _movement;

    private PlayerClassData _classData;

	private HP _health;
	private int _mana;
	private int _manaRegeneration;
	private int _spellpower;
    private int _speed;

    void Start() {
		PlayerClassManager.GetClasses((Dictionary<string, PlayerClassData> classData) => {
			SetClass(classData["mage"]);
		});

		EventBus.Instance.OnWaveStarted += OnWaveChanged;
	}

	public void SetClass(PlayerClassData classData) {
		_classData = classData;
	}

	private void Attack() {
		EventBus.Instance.InvokePlayerShoot();

		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
	}

	private void OnWaveChanged(int newWaveIndex, int _) {
		_classData.CalculatePlayerStatsForWave(
			newWaveIndex,
			out int hpValue,
			out _mana,
			out _manaRegeneration,
			out _spellpower,
			out _speed
		);

		_health = new(hpValue);
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
		Move(new Vector2(_movement.x, 0.0f) * (Time.fixedDeltaTime * _speed));
		Move(new Vector2(0.0f, _movement.y) * (Time.fixedDeltaTime * _speed));
	}

	public void Hit(Damage damage) {
		_health.TakeDamage(damage);
	}

    void Die() {
		EventBus.Instance.InvokePlayerDeath();
    }

}
