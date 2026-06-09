using System;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackRadius))]
public class EnemyInstance :
	MonoBehaviour, IHittable
{

	private HP _health;

    public string targetTag;
    private int _speed;
    private int _damage;
    private bool _dead = false;

	private AttackRadius _attackRadius;

	private Transform _target;

	private static GameObject _enemyPrefab;

	public static void Instantiate(EnemyStatData statData, Action<EnemyInstance> onInstantiation) {
		if(_enemyPrefab == null) {
			_LoadPrefab(_Instantiate);

			return;
		}

		_Instantiate();

		return;

		void _LoadPrefab(Action onLoaded) {
			AssetManager.Instance.LoadPrefab("enemy", (loadedPrefab) => {
				_enemyPrefab = loadedPrefab;
				onLoaded.Invoke();
			});

			return;
		}

		void _Instantiate() {
			GameObject newEnemyObject = GameObject.Instantiate(_enemyPrefab);
			EnemyInstance newEnemy = newEnemyObject.GetComponent<EnemyInstance>();

			newEnemy._damage = statData.Damage;
			newEnemy._health = new(statData.HP, newEnemy.GetComponent<HealthBar>());
			newEnemy._speed = statData.Speed;

			newEnemy.GetComponent<AttackRadius>().OnRadiusEntered += newEnemy.TryAttack;

			newEnemy.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrieveEnemySprite(statData.SpriteIndex);

			onInstantiation.Invoke(newEnemy);

			return;
		}
	}

	public Team GetTeam() {
		return Team.ENEMY;
	}
    private void Start() {
		_attackRadius = GetComponent<AttackRadius>();

        _target = GameObject.FindWithTag(targetTag).transform;
        _health.OnExpended += Die;
    }

	private void Move(Vector2 direction) {
		List<RaycastHit2D> hits = new();
		ContactFilter2D filter = new();
		filter.useTriggers = false;
		int collisionCount = GetComponent<Rigidbody2D>().Cast(direction, filter, hits, 1.0f);
		if(collisionCount == 0) { transform.Translate(direction * (_speed * Time.deltaTime)); }
	}

    private void Update() {
        Vector3 direction = (_target.position - transform.position).normalized;

		Move(new Vector2(direction.x, 0.0f));
		Move(new Vector2(0.0f, direction.y));
    }

	private void TryAttack(GameObject potentialTarget) {
		// TODO - factor this and Attack's similar lambda
		if(potentialTarget.CompareTag("Player")) {
			Attack(potentialTarget);
		}
	}

	private void Attack(GameObject target) {
		IHittable hittable = target.GetComponent<IHittable>();
		if(hittable != null) { hittable.Hit(new Damage(5, Damage.Type.PHYSICAL)); }

		_attackRadius.OnRadiusEntered -= TryAttack;
		Timer cooldown = new(2000);
		cooldown.Elapsed += (_, _) => {
			_attackRadius.OnRadiusEntered += TryAttack;

			// TODO - factor this lambda and TryAttack's similar code
			_attackRadius.FindWithFilter(
				(GameObject gameObject) => {
					return gameObject.CompareTag("Player");
				},
				(List<GameObject> filterResults) => {
				// TODO - allow for more robust selection
					if(filterResults.Count > 0) {
						Attack(filterResults[0]);
					}
				}
			);
		};
		cooldown.AutoReset = false;
		cooldown.Enabled = true;
	}

	public void Hit(Damage damage) {
		_health.TakeDamage(damage);

		EventBus.Instance.DoOnKill();
		EventBus.Instance.DoDamage(transform.position, damage, this);
	}

    private void Die() {
        if (!_dead) {
			EventBus.Instance.InvokeEnemyDefeated();

            _dead = true;
            Destroy(gameObject);
        }
    }

}
