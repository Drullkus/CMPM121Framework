using System;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionQueue : MonoBehaviour {

	public static ExecutionQueue Instance;

	private Queue<Action> _queue = new();

	public void Enqueue(Action action) { _queue.Enqueue(action); }

	private void Awake() {
		if(Instance != null) {
			if(Instance != this) { Destroy(gameObject); }
			return;
		}

		Instance = this;
	}

	private void Update() {
		while(_queue.Count > 0) {
			_queue.Dequeue().Invoke();
		}
	}

}
