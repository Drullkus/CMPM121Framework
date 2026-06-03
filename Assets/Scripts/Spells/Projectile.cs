using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData {
	
	private Dictionary<string, int> _variableDictionary;

	public float speed = 0.0f;
	public float lifetime = 1.0f;

	public string trajectory = "straight";

	public Action<IHittable, IHittable> onHit;

	public Team team = Team.PLAYER;

	public int spriteIndex = 0;

	public ProjectileData() {}

	public ProjectileData SetRPNDictionary(Dictionary<string, int> dictionary) {
		_variableDictionary = dictionary;

		return this;
	}

	private bool AssertDictionarySet() {
		if(_variableDictionary == null) {
			Debug.LogError("You must SetRPNDictionary on ProjectileData before setting its other properties!");
			return false;
		}

		return true;
	}

	public ProjectileData SetSpeed(string rpnSpeed) {
		if(AssertDictionarySet()) {
			speed = RPNEvaluator.RPNEvaluator.Evaluatef(rpnSpeed, _variableDictionary);
		}

		return this;
	}

	public ProjectileData SetLifetime(string rpnLifetime) {
		if(AssertDictionarySet()) {
			lifetime = RPNEvaluator.RPNEvaluator.Evaluatef(rpnLifetime, _variableDictionary);
		}

		return this;
	}

	public ProjectileData SetTrajectory(string trajectory) {
		this.trajectory = trajectory;
		return this;
	}

	public ProjectileData SetOnHit(Action<IHittable, IHittable> onHit) {
		this.onHit = onHit;
		return this;
	}

	public ProjectileData SetTeam(Team team) {
		this.team = team;
		return this;
	}

	public ProjectileData SetSpriteIndex(int spriteIndex) {
		this.spriteIndex = spriteIndex;
		return this;
	}

}

public abstract class ProjectileMovement {

	public abstract void Move(Transform transform);

}

public class ProjectileMovementStraight : ProjectileMovement {

	public float speed = 1.0f;

	public ProjectileMovementStraight SetSpeed(float speed) {
		this.speed = speed;
		return this;
	}

	public override void Move(Transform transform) {
		transform.Translate(speed * Time.deltaTime * Vector3.forward);
	}

}

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    
	private ProjectileTrajectory _trajectory = ProjectileTrajectory.STRAIGHT;
	public Team team;

	private ProjectileMovement _projectileMovement;

	public static Projectile Spawn(Vector2 spawnPosition, Vector2 spawnDirection, ProjectileData spawnData) {
		GameObject projectilePrefab = AssetManager.Instance.projectilePrefab;

		Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(spawnDirection.y, spawnDirection.x) * Mathf.Rad2Deg);

		Projectile newProjectile = Instantiate(projectilePrefab, spawnPosition, rotation).GetComponent<Projectile>();

		newProjectile.team = spawnData.team;

		newProjectile.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrieveSpellSprite(spawnData.spriteIndex);

		Enum.TryParse(spawnData.trajectory, out newProjectile._trajectory);

		switch(newProjectile._trajectory) {
			case ProjectileTrajectory.STRAIGHT:
				newProjectile._projectileMovement = new ProjectileMovementStraight().SetSpeed(spawnData.speed);
				break;
			default:
				break;
		}

		return newProjectile;
	}

	private void Update() {
		_projectileMovement.Move(transform);
	}

}
