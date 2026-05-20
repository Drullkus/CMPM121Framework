using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Reward : MonoBehaviour {

	[SerializeField]
	private List<GameObject> _statDisplaySlots;

	private void Awake() {
		UIObject uiObject = new(
			UIState.REWARD,
			Show,
			() => { gameObject.SetActive(false); }
		);

		EventBus.Instance.RegisterUIObject(uiObject);
	}

	private void Show() {
		List<string> stats = WaveStatTracker.Instance.GetRandomFormattedStats(_statDisplaySlots.Count);

		foreach(GameObject statDisplaySlot in _statDisplaySlots) {
			statDisplaySlot.SetActive(false);
		}

		for(int i = 0; i < stats.Count; i++) {
			_statDisplaySlots[i].GetComponent<TextMeshProUGUI>().text = stats[i];

			_statDisplaySlots[i].SetActive(true);
		}

		gameObject.SetActive(true);
	}

}
