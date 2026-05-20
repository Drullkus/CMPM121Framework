using UnityEngine;

public class EnemyController : MonoBehaviour {

    public string targetTag;
    public int speed;
    public int damage;
    public Hittable hp;
    public HealthBar healthui;
    public bool dead;

	private Transform _target;

    public float lastAttack;

    private void Start() {
        _target = GameObject.FindWithTag(targetTag).transform;
        hp.OnDeath += Die;
        // healthui.SetHealth(hp);
    }

    private void Update() {
        Vector3 direction = _target.position - transform.position;
        if (direction.magnitude < 2f) {
            // DoAttack();
        } else {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }
    }
    
    private void DoAttack() {
        if (lastAttack + 2 < Time.time) {
            lastAttack = Time.time;
			// TODO - don't depend on _target having a PlayerController
            _target.gameObject.GetComponent<PlayerController>().hp.Damage(new Damage(damage, Damage.Type.PHYSICAL));
        }
    }

    private void Die() {
        if (!dead) {
            dead = true;
            Destroy(gameObject);
        }
    }

}
