using System;
using System.Collections.Generic;
using UnityEngine;


public class AttackRadius : MonoBehaviour {

	private List<GameObject> _overlappingWith = new();

	private float _radius = 2.0f;

	private void OnTriggerEnter(Collider2D other) {
		_overlappingWith.Add(other.gameObject);
	}

	private void OnTriggerExit(Collider2D other) {
		_overlappingWith.Remove(other.gameObject);
	}

	public List<GameObject> FindWithFilter(Func<GameObject, bool> filter) {
		List<GameObject> result = new();

		foreach(GameObject gameObject in _overlappingWith) {
			if(filter(gameObject)) { result.Add(gameObject); }
		}

		return result;
	}
    
}
