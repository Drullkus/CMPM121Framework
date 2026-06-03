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

	private bool _movementBlocked = true;

    void Start() {
		PlayerClassManager.GetClasses((Dictionary<string, PlayerClassData> classData) => {
			SetClass(classData["mage"]);
			SetStats(0);
		});

		EventBus.Instance.OnWaveStarted += OnWaveChanged;

		EventBus.Instance.OnCountdownStarted += () => { _movementBlocked = false; };
		EventBus.Instance.OnWaveEnded += () => {
			_movementBlocked = true;
			_movement = Vector2.zero;
		};
		EventBus.Instance.OnPlayerDeath += () => {
			_movementBlocked = true;
			_movement = Vector2.zero;
		};

		_health.OnExpended += Die;

		UI.SpellBarManager spellBarManager = FindFirstObjectByType<UI.SpellBarManager>();
		spellBarManager.AddSpell(SpellReader.Instance.randomSpellBase);
	}

	public void SetClass(PlayerClassData classData) {
		_classData = classData;
	}

	private void SetStats(int waveIndex) {
		_classData.CalculatePlayerStatsForWave(
			waveIndex,
			out int hpValue,
			out _mana,
			out _manaRegeneration,
			out _spellpower,
			out _speed
		);

		_health = new(hpValue, GetComponent<HealthBar>());
	}

	public void OnAttack(InputAction.CallbackContext context) {
		if(Mouse.current.leftButton.wasPressedThisFrame) { return; }

		EventBus.Instance.InvokePlayerShoot();

		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
		Vector2 castDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;

		UI.SpellBarManager spellBarManager = FindFirstObjectByType<UI.SpellBarManager>();
		spellBarManager.activeSpell.Cast(transform.position, castDirection, Team.PLAYER, 100, 1);
	}

	private void OnWaveChanged(int newWaveIndex, int _) {
		SetStats(newWaveIndex);
	}

	public void OnMove(InputAction.CallbackContext context) {
		if(_movementBlocked) { return; }
		_movement = context.ReadValue<Vector2>();
	}

	private void Move(Vector2 direction) {
		List<RaycastHit2D> hits = new List<RaycastHit2D>();
		ContactFilter2D filter = new();
		filter.useTriggers = false;
		
		int collisionCount = GetComponent<Rigidbody2D>().Cast(direction, filter, hits, direction.magnitude * 2.0f);

		if(collisionCount > 0) { return; }

		transform.Translate(direction);
	}

	private void FixedUpdate() {
		Move(new Vector2(_movement.x, 0.0f) * (Time.fixedDeltaTime * _speed));
		Move(new Vector2(0.0f, _movement.y) * (Time.fixedDeltaTime * _speed));
	}

	public void Hit(Damage damage) {
		_health.TakeDamage(damage);

		EventBus.Instance.DoDamage(transform.position, damage, this);
	}

    void Die() {
		EventBus.Instance.InvokePlayerDeath();
    }

}
