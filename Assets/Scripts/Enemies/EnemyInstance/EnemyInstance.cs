using UnityEngine;

public class EnemyInstance :
	MonoBehaviour, IHittable
{

	private HP _hp;

    public string targetTag;
    public int speed;
    public int damage;
    // public HealthBar healthui;
    public bool dead;

	private Transform _target;

    public float lastAttack;

    private void Start() {
        _target = GameObject.FindWithTag(targetTag).transform;
        _hp.OnExpended += Die;
        // healthui.SetHealth(hp);
    }

    private void Update() {
        Vector3 direction = _target.position - transform.position;
        if (direction.magnitude < 2f) {
            DoAttack();
        } else {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }
    }
    
    private void DoAttack() {
        if (lastAttack + 2 < Time.time) {
            lastAttack = Time.time;
            _target.gameObject.GetComponent<IHittable>().Hit(new Damage(damage, Damage.Type.PHYSICAL));
        }
    }

	public void Hit(Damage damage) {
		_hp.TakeDamage(damage);
	}

    private void Die() {
        if (!dead) {
            dead = true;
            Destroy(gameObject);
        }
    }

}
