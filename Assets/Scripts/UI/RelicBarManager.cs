using Relic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class RelicBarManager : MonoBehaviour {
		[SerializeField] private GameObject relicSlotPrefab;
		private List<GameObject> relicSlots = new();
		public HashSet<string> claimedRelics = new();

        public void AddRelic(RelicData relicPrototype) {
			GameObject newRelicSlot = Instantiate(relicSlotPrefab, this.transform.position + new Vector3(40 * relicSlots.Count, 0, 0), Quaternion.identity, this.gameObject.transform);

			newRelicSlot.GetComponentInChildren<Image>().sprite = SpriteManager.Instance.RetrieveRelicSprite(relicPrototype.Sprite);

			relicSlots.Add(newRelicSlot);
			claimedRelics.Add(relicPrototype.Name);

			new Relic.Relic(relicPrototype);
		}
    }
}