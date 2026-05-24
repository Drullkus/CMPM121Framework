using System;
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
    private bool _dead;

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

    private void Start() {
		_attackRadius = GetComponent<AttackRadius>();

        _target = GameObject.FindWithTag(targetTag).transform;
        _health.OnExpended += Die;
    }

    private void Update() {
        Vector3 direction = (_target.position - transform.position).normalized;

		List<RaycastHit2D> hits = new List<RaycastHit2D>();
		ContactFilter2D filter = new();
		filter.useTriggers = false;
		int collisionCount = GetComponent<Rigidbody2D>().Cast(direction, filter, hits, 2.0f);

		if(collisionCount == 0) { transform.Translate(direction * (_speed * Time.deltaTime)); }
    }

	private void TryAttack(GameObject potentialTarget) {
		if(potentialTarget.CompareTag("Player")) {
			Attack(potentialTarget);
		}
	}

	private void Attack(GameObject target) {
		IHittable hittable = target.GetComponent<IHittable>();
		if(hittable != null) { hittable.Hit(new Damage(5, Damage.Type.PHYSICAL)); }
	}

	public void Hit(Damage damage) {
		_health.TakeDamage(damage);
	}

    private void Die() {
        if (!_dead) {
            _dead = true;
            Destroy(gameObject);
        }
    }

}
