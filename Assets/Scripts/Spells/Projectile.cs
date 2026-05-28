using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    
	public Team team;

	private ProjectileTrajectory trajectory;

	public static void Spawn(Vector2 spawnPosition, ProjectileTrajectory trajectory, int spriteIndex, Team team, Action<Projectile> onSpawn) {
		AssetManager.Instance.LoadPrefab("projectile", (loadedPrefab) => {
			Projectile newProjectile = Instantiate(loadedPrefab, spawnPosition, Quaternion.identity).GetComponent<Projectile>();

			newProjectile.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrieveSpellSprite(spriteIndex);

			newProjectile.team = team;
			newProjectile.trajectory = trajectory;

			onSpawn.Invoke(newProjectile);
		});
	}

	private void Update() {
		
	}

}
