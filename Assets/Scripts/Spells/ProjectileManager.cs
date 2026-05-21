using UnityEngine;
using System;

public class ProjectileManager : MonoBehaviour {

    public GameObject[] projectiles;

    public void CreateProjectile(
		int which,
		string trajectory,
		Vector3 where,
		Vector3 direction,
		float speed,
		Action<IHittable, Vector3> onHit
	) {
        GameObject newProjectile = Instantiate(
			projectiles[which],
			where + direction.normalized*1.1f,
			Quaternion.Euler(
				0,0,Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg
			)
		);
        newProjectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        newProjectile.GetComponent<ProjectileController>().OnHit += onHit;
    }

	// too many params!
	// TODO - ProjectileSpawnDescriptor class (or maybe use SpellBuilder?)
    public void CreateProjectile(
		int which,
		string trajectory,
		Vector3 where,
		Vector3 direction,
		float speed,
		Action<IHittable, Vector3> onHit,
		float lifetime
	) {
        GameObject newProjectile = Instantiate(projectiles[which], where + direction.normalized * 1.1f, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        newProjectile.GetComponent<ProjectileController>().movement = MakeMovement(trajectory, speed);
        newProjectile.GetComponent<ProjectileController>().OnHit += onHit;
        newProjectile.GetComponent<ProjectileController>().SetLifetime(lifetime);
    }

    public ProjectileMovement MakeMovement(string name, float speed) {
        if (name == "straight") {
            return new StraightProjectileMovement(speed);
        }

        if (name == "homing") {
            return new HomingProjectileMovement(speed);
        }

        if (name == "spiraling") {
            return new SpiralingProjectileMovement(speed);
        }

        return null;
    }

}
