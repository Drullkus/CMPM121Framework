using Relic;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UIScreens {
    public class RelicRewardManager : MonoBehaviour {
        [SerializeField] private GameObject relic1;
        [SerializeField] private GameObject relic2;
        [SerializeField] private GameObject relic3;

        public void RollRelics() {
			relic1.SetActive(false);
			relic2.SetActive(false);
			relic3.SetActive(false);

			Debug.Log("Rolling relics");

			List<RelicData> randomRelics = RelicManager.Instance.GetRandomRelicOptions(new HashSet<string>());

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
		}
    }
}