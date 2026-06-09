using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Relic {
    public class RelicRewardManager : MonoBehaviour {
        [SerializeField] private GameObject relic1;
        [SerializeField] private GameObject relic2;
        [SerializeField] private GameObject relic3;

        [SerializeField] private RelicBarManager relicBar;

        [SerializeField] private GameManager gameManager;

        public void RollRelics() {
			HideRelicButtons();

			if ((gameManager.getWave() % 3) != 1) { // Rewards are first shown when getWave() returns 2, never shown because wave 1 ends before rewards are shown
				return;
			}
			// Debug.Log($"Showing relics before wave {gameManager.getWave()}"); // Show relics if wave 3 completed, going before 4

			List<RelicData> randomRelics = RelicManager.Instance.GetRandomRelicOptions(relicBar.claimedRelics);

			if(randomRelics.Count > 0) {
				BindRelicInfo(relic1, randomRelics[0]);
			}
			if(randomRelics.Count > 1) {
				BindRelicInfo(relic2, randomRelics[1]);
			}
			if(randomRelics.Count > 2) {
				BindRelicInfo(relic3, randomRelics[2]);
			}
		}

		private void BindRelicInfo(GameObject relicButton, RelicData relic) {
			relicButton.SetActive(true);
			relicButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{relic.Name}\n{relic.Trigger.Description}\n{relic.Effect.Description}";
			relicButton.GetComponentInChildren<Image>().sprite = SpriteManager.Instance.RetrieveRelicSprite(relic.Sprite);
			relicButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
			relicButton.GetComponentInChildren<Button>().onClick.AddListener(() => {
				HideRelicButtons();
				relicBar.AddRelic(relic);
			});
		}

		private void HideRelicButtons() {
			relic1.SetActive(false);
			relic2.SetActive(false);
			relic3.SetActive(false);
		}
    }
}