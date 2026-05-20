using UnityEngine;

public class EnemyController : MonoBehaviour {

    public string targetTag;
    public int speed;
    public int damage;
    public Hittable hp;
    public HealthBar healthui;
    public bool dead;

	private Transform _target;

    public float last_attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _target = GameObject.FindWithTag(targetTag).transform;
        hp.OnDeath += Die;
        // healthui.SetHealth(hp);
    }

    // Update is called once per frame
    void Update() {
        Vector3 direction = _target.position - transform.position;
        if (direction.magnitude < 2f) {
            // DoAttack();
        } else {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }
    }
    
    void DoAttack() {
        if (last_attack + 2 < Time.time) {
            last_attack = Time.time;
			// TODO - don't depend on _target having a PlayerController
            _target.gameObject.GetComponent<PlayerController>().hp.Damage(new Damage(damage, Damage.Type.PHYSICAL));
        }
    }


    void Die() {
        if (!dead) {
            dead = true;
            Destroy(gameObject);
        }
    }

}
