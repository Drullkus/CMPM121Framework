using Relic;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UI.UIScreens;

public class Reward : MonoBehaviour {

	[SerializeField]
	private List<TextMeshProUGUI> _statDisplayLabels;

	[SerializeField]
	private Button _nextWaveButton;

	[SerializeField]
	private GameObject _spellRewardButton;

	[SerializeField] private RelicRewardManager _relicRewards;

	private void Start() {
		UIScreen uiScreen = new(
			UIState.REWARD,
			Show,
			() => { gameObject.SetActive(false); }
		);

		_nextWaveButton.onClick.AddListener(EventBus.Instance.RequestNextWave);
		_nextWaveButton.onClick.AddListener(EventBus.Instance.CloseUIScreen);

		EventBus.Instance.RegisterUIScreen(uiScreen);

		gameObject.SetActive(false);
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

		_spellRewardButton.GetComponent<GiveSpellReward>().RollOption();
		_relicRewards.RollRelics();

		gameObject.SetActive(true);
	}

}
