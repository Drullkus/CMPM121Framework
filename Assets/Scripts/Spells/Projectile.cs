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

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    
	private ProjectileTrajectory trajectory;
	public Team team;

	public static Projectile Spawn(Vector2 spawnPosition, ProjectileData spawnData) {
		GameObject projectilePrefab = AssetManager.Instance.projectilePrefab;

		Projectile newProjectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity).GetComponent<Projectile>();

		newProjectile.team = spawnData.team;

		Enum.TryParse(spawnData.trajectory, out newProjectile.trajectory);
		newProjectile.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrieveSpellSprite(spawnData.spriteIndex);

		return newProjectile;
	}

	private void Update() {
		
	}

}
