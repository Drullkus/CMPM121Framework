using UnityEngine;
using System;
using System.Collections;

public class ProjectileController : MonoBehaviour {

    public float lifetime;
    public event Action<IHittable, Vector3> OnHit;
    public ProjectileMovement movement;

    void Update() {
        movement.Movement(transform);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("projectile")) { return; };
        if (collision.gameObject.CompareTag("unit")) {
            IHittable enemy = collision.gameObject.GetComponent<IHittable>();

            if (enemy != null) {
                OnHit(enemy, transform.position);
                EventBus.Instance.InvokeEnemyHit();
            } else {
				IHittable player = collision.gameObject.GetComponent<IHittable>();
                if (player != null) {
                    OnHit(player, transform.position);
                }
            }

        }

        Destroy(gameObject);
    }

    public void SetLifetime(float lifetime) {
        StartCoroutine(Expire(lifetime));
    }

    IEnumerator Expire(float lifetime) {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

}
