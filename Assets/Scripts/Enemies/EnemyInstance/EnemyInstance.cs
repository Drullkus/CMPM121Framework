using UnityEngine;

public class EnemyInstance :
	MonoBehaviour, IHittable
{

	private HP _health;

    private string _targetTag;
    private int _speed;
    private int _damage;
    private bool _dead;

	private Transform _target;

    public float lastAttack;

    private void Start() {
		_health = new(0);

        _target = GameObject.FindWithTag(_targetTag).transform;
        _health.OnExpended += Die;
    }

    private void Update() {
        Vector3 direction = _target.position - transform.position;
        if (direction.magnitude < 2f) {
            DoAttack();
        }
    }
    
    private void DoAttack() {
        if (lastAttack + 2 < Time.time) {
            lastAttack = Time.time;
            _target.gameObject.GetComponent<IHittable>().Hit(new Damage(_damage, Damage.Type.PHYSICAL));
        }
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
