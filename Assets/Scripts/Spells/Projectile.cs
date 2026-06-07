using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData {
	
	private Dictionary<string, int> _variableDictionary;

	public float speed = 10.0f;
	public float lifetime = 1.0f;

	public string trajectory = "straight";

	public Damage damage;

	public Action<IHittable, IHittable, Vector2> onHit;

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

	public ProjectileData SetDamageAmount(string rpnDamageAmount) {
		damage = new Damage((int)RPNEvaluator.RPNEvaluator.Evaluatef(rpnDamageAmount, _variableDictionary), Damage.Type.ARCANE);
		return this;
	}

	public ProjectileData SetOnHit(Action<IHittable, IHittable, Vector2> onHit) {
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
		transform.Translate(speed * Time.deltaTime * Vector2.right);
	}

}

public class ProjectileMovementHoming : ProjectileMovement {

	public float speed = 1.0f;

	private Team _team;
	private Transform _target;

	private const float _searchRadius = 4.0f;

	public ProjectileMovementHoming SetSpeed(float speed) {
		this.speed = speed;
		return this;
	}

	public ProjectileMovementHoming SetTeam(Team team) {
		_team = team;
		return this;
	}

	public override void Move(Transform transform) {
		if(_target == null) {
			Collider2D[] collidersInRadius = Physics2D.OverlapCircleAll(transform.position, _searchRadius, Physics.AllLayers);

			float lowestDistanceSquared = _searchRadius + 1.0f;
			Transform candidate = null;

			foreach(Collider2D collider in collidersInRadius) {
				IHittable hittable = collider.GetComponent<IHittable>();

				if(hittable == null) { continue; }
				if(hittable.GetTeam() == _team) { continue; }

				float distanceSquared = (transform.position - collider.transform.position).sqrMagnitude;
				if(distanceSquared < lowestDistanceSquared) {
					candidate = collider.transform;
					lowestDistanceSquared = distanceSquared;
				}
			}

			_target = candidate;
		}

		transform.Translate(speed * Time.deltaTime * Vector2.right);

		if(_target == null) { return; }

		Vector2 delta = _target.position - transform.position;
		Vector2 direction = delta.normalized;

		transform.rotation = Quaternion.AngleAxis(
			Mathf.LerpAngle(
				transform.rotation.z * Mathf.Rad2Deg,
				Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,
				0.6f
			),
			Vector3.forward
		);

		if(delta.magnitude > 2.0f * _searchRadius) { _target = null; }
	}

}

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour {
    
	private GameObject _source;

	private ProjectileTrajectory _trajectory = ProjectileTrajectory.STRAIGHT;
	public Team team;

	private Action<IHittable, IHittable, Vector2> _onHit;

	private ProjectileMovement _projectileMovement;

	private Damage _damage = new(0, Damage.Type.ARCANE);

	public static Projectile Spawn(GameObject source, Vector2 spawnPosition, Vector2 spawnDirection, ProjectileData spawnData) {
		GameObject projectilePrefab = AssetManager.Instance.projectilePrefab;

		Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(spawnDirection.y, spawnDirection.x) * Mathf.Rad2Deg);
		
		Vector2 offsetPosition = spawnPosition + spawnDirection * 1.5f;

		Projectile newProjectile = Instantiate(projectilePrefab, offsetPosition, rotation).GetComponent<Projectile>();

		newProjectile._source = source;
		newProjectile.team = spawnData.team;

		if(spawnData.onHit != null) { newProjectile._onHit = spawnData.onHit; }

		if(spawnData.damage != null) { newProjectile._damage = spawnData.damage; }

		newProjectile.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.RetrieveSpellSprite(spawnData.spriteIndex);

		Enum.TryParse(spawnData.trajectory, out newProjectile._trajectory);

		switch(newProjectile._trajectory) {
			case ProjectileTrajectory.STRAIGHT:
				newProjectile._projectileMovement = new ProjectileMovementStraight().SetSpeed(spawnData.speed);
				break;
			case ProjectileTrajectory.HOMING:
				newProjectile._projectileMovement = new ProjectileMovementHoming().SetSpeed(spawnData.speed);
				break;
			default:
				break;
		}

		return newProjectile;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		IHittable hittable = collision.gameObject.GetComponent<IHittable>();

		if(hittable != null && hittable.GetTeam() != team) {
			hittable.Hit(_damage);

			_onHit?.Invoke(_source.GetComponent<IHittable>(), hittable, collision.contacts[0].point);
		} else if(hittable == null) {
			_onHit?.Invoke(_source.GetComponent<IHittable>(), null, collision.contacts[0].point);
		}

		if(hittable != null && hittable.GetTeam() == team) { return; }

		Destroy(gameObject);
	}

	private void Update() {
		_projectileMovement.Move(transform);
	}

}
