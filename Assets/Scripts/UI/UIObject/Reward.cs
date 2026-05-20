using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Reward : MonoBehaviour {

	[SerializeField]
	private List<TextMeshProUGUI> _statDisplayLabels;

	private void Awake() {
		UIObject uiObject = new(
			UIState.REWARD,
			Show,
			() => { gameObject.SetActive(false); }
		);

		EventBus.Instance.RegisterUIObject(uiObject);
	}

	private void Show() {
		List<string> stats = WaveStatTracker.Instance.GetRandomFormattedStats(_statDisplayLabels.Count);

		foreach(TextMeshProUGUI statDisplaySlot in _statDisplayLabels) {
			statDisplaySlot.gameObject.SetActive(false);
		}

		for(int i = 0; i < stats.Count; i++) {
			_statDisplayLabels[i].text = stats[i];

			_statDisplayLabels[i].gameObject.SetActive(true);
		}

		gameObject.SetActive(true);
	}

}
