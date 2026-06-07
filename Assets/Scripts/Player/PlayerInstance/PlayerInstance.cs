using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UI;

[RequireComponent(typeof(ManaBar))]
public class PlayerInstance :
	MonoBehaviour, IHittable
{

	private Vector2 _movement;

    private PlayerClassData _classData;

	private HP _health;
	private int _maxMana;
	private int _mana;
	private int _manaRegeneration;
	private int _spellpower;
    private int _speed;

	private ManaBar _manaBar;

	private int _waveIndex;

	private bool _movementBlocked = true;

	private Timer _manaRegenTimer;
	
	[SerializeField] private SpellBarManager _spellBarManager;
	[SerializeField] private InputActionAsset playerControls;

	private InputAction toggleSpell;
	private InputAction spell1;
	private InputAction spell2;
	private InputAction spell3;
	private InputAction spell4;

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

		_manaRegenTimer = new(1000);
		_manaRegenTimer.Elapsed += (_, _) => { ExecutionQueue.Instance.Enqueue(RestoreSomeMana); }; 
		_manaRegenTimer.Enabled = true;

		_health.OnExpended += Die;

		_manaBar = GetComponent<ManaBar>();

		UI.SpellBarManager spellBarManager = FindFirstObjectByType<UI.SpellBarManager>();
		spellBarManager.AddSpell(SpellReader.Instance.randomSpellBase);
	}

    void Awake() {
	    InputActionMap mapReference = playerControls.FindActionMap("Player");
	    
	    toggleSpell = mapReference.FindAction("ChangeSpell");
	    spell1 = mapReference.FindAction("Spell1");
	    spell2 = mapReference.FindAction("Spell2");
	    spell3 = mapReference.FindAction("Spell3");
	    spell4 = mapReference.FindAction("Spell4");

	    toggleSpell.performed += OnToggleSpell;
	    spell1.performed += OnSelectSpell;
	    spell2.performed += OnSelectSpell;
	    spell3.performed += OnSelectSpell;
	    spell4.performed += OnSelectSpell;
	    
	    playerControls.FindActionMap("Player").Enable();
    }

	public Team GetTeam() {
		return Team.PLAYER;
	}

	public void SetClass(PlayerClassData classData) {
		_classData = classData;
	}

	private void SetStats(int waveIndex) {
		_classData.CalculatePlayerStatsForWave(
			waveIndex,
			out int hpValue,
			out _maxMana,
			out _manaRegeneration,
			out _spellpower,
			out _speed
		);

		_mana = _maxMana;

		_health = new(hpValue, GetComponent<HealthBar>());
	}

	public void OnAttack(InputAction.CallbackContext context) {
		if(Mouse.current.leftButton.wasPressedThisFrame) { return; }

		EventBus.Instance.InvokePlayerShoot();

		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
		Vector2 castDirection = (mouseWorldPosition - (Vector2)transform.position).normalized;

		UI.SpellBarManager spellBarManager = FindFirstObjectByType<UI.SpellBarManager>();

		Spell activeSpell = spellBarManager.activeSpell;
		int manaCost = 0;

		List<(string, SpellTrait)> wrappedCost = activeSpell.GetTraits(new List<string>(){"manaCost"});

		if(wrappedCost.Count > 0) {
			Dictionary<string, int> castVariables = new() {
				[ "power" ] = _spellpower,
				[ "wave" ] = _waveIndex,
			};

			string unevaluatedCost = wrappedCost[0].Item2.traitValue;
			manaCost = (int)(RPNEvaluator.RPNEvaluator.Evaluatef(unevaluatedCost, castVariables));
		}

		if(manaCost > _mana) { return; }

		_mana -= manaCost;
		spellBarManager.activeSpell.Cast(gameObject, castDirection, Team.PLAYER, _spellpower, 1);

		_manaBar.SetMana((float)_mana / (float)_maxMana);
	}

	private void OnWaveChanged(int newWaveIndex, int _) {
		_waveIndex = newWaveIndex;
		SetStats(newWaveIndex);

		RestoreAllMana();
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

	public void OnToggleSpell(InputAction.CallbackContext context) {
		if (context.performed && context.control is KeyControl keyControl && keyControl.keyCode == Key.Tab) {
			_spellBarManager.ToggleSpellSlot();
		}
	}

	public void OnSelectSpell(InputAction.CallbackContext context) {
		if (context.performed && context.control is KeyControl keyControl) {
			if (keyControl.keyCode == Key.Digit1) {
				_spellBarManager.ChooseSpell(1);
			} else if (keyControl.keyCode == Key.Digit2) {
				_spellBarManager.ChooseSpell(2);
			} else if (keyControl.keyCode == Key.Digit3) {
				_spellBarManager.ChooseSpell(3);
			} else if (keyControl.keyCode == Key.Digit4) {
				_spellBarManager.ChooseSpell(4);
			}
		}
	}

	private void RestoreSomeMana() {
		_mana += _manaRegeneration;
		if(_mana > _maxMana) { _mana = _maxMana; }

		_manaBar.SetMana((float)_mana / (float)_maxMana);
	}

	private void RestoreAllMana() {
		_mana = _maxMana;

		_manaBar.SetMana(1.0f);
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
