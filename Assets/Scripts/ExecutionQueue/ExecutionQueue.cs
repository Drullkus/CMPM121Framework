using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutionQueue : MonoBehaviour {

	public static ExecutionQueue Instance;

	private ConcurrentQueue<Action> _queue = new();

	public void Enqueue(Action action) { _queue.Enqueue(action); }

	private void Awake() {
		if(Instance != null) {
			if(Instance != this) { Destroy(gameObject); }

			SceneManager.sceneLoaded += (_, _) => {
				_queue.Clear();
			};

			return;
		}

		Instance = this;
	}

	private void Update() {
		while(_queue.TryDequeue(out Action action)) {
			action.Invoke();
		}
	}

}
