using Relic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Relic {
    public class RelicBarManager : MonoBehaviour {
		[SerializeField] private GameObject relicSlotPrefab;
		private List<GameObject> relicSlots = new();
		public HashSet<string> claimedRelics = new();

		public void Start() {
			// GrantRelic("Green Gem");
			// GrantRelic("Jade Elephant");
			// GrantRelic("Golden Mask");
			// GrantRelic("Cursed Scroll");
			// GrantRelic("Cheese Shield");
			// GrantRelic("Healer's Gauntlet");
			// GrantRelic("Quickscope");
		}

        public void AddRelic(RelicData relicPrototype) {
	        int offset = GetRelicPlacementOffset();
	        //Debug.Log($"Offset: {offset}");
			GameObject newRelicSlot = Instantiate(relicSlotPrefab, this.transform.position + new Vector3(offset, 0, 0), Quaternion.identity, this.gameObject.transform);

			newRelicSlot.GetComponentInChildren<Image>().sprite = SpriteManager.Instance.RetrieveRelicSprite(relicPrototype.Sprite);

			relicSlots.Add(newRelicSlot);
			claimedRelics.Add(relicPrototype.Name);

			new Relic(relicPrototype);
		}

		private int GetRelicPlacementOffset() {
			int relicIndex = relicSlots.Count;
			// Places first in middle then alternates placement on right then left
			// Relic index == # of relics owned before obtaining
			if (relicIndex == 0) return 0;
			
			int sign = (relicIndex % 2) * 2 - 1; // right is signed positive, left is signed negative
			int offsetShift = (relicIndex + 1) / 2; // Actual offset from index=0
			
			return (sign * offsetShift) * 40; // 40 pixels of spacing between object origins
		}

		public void GrantRelic(string name) {
			AddRelic(RelicManager.Instance.GetRelicData(name));
		}
    }
}