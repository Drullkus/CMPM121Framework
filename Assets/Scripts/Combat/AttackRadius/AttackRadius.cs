using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AttackRadius : MonoBehaviour {

	private List<GameObject> _overlappingWith = new();

	public event Action<GameObject> OnRadiusEntered;
	public event Action<GameObject> OnRadiusExited;

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject == this) { return; }
		_overlappingWith.Add(other.gameObject);
		OnRadiusEntered?.Invoke(other.gameObject);
	}

	private void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject == this) { return; }
		_overlappingWith.Remove(other.gameObject);
		OnRadiusExited?.Invoke(other.gameObject);
	}

	public void FindWithFilter(Func<GameObject, bool> filter, Action<List<GameObject>> onFiltered) {
		ExecutionQueue.Instance.Enqueue(() => {
			List<GameObject> result = new();

			foreach(GameObject gameObject in _overlappingWith) {
				if(filter(gameObject)) { result.Add(gameObject); }
			}

			onFiltered.Invoke(result);
		});
	}

	private void SetRadius(float newRadius) {
		GetComponent<CircleCollider2D>().radius = newRadius;
	}
    
}
