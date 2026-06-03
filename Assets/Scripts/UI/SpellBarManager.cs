using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class SpellBarManager : MonoBehaviour {

        [SerializeField] private List<GameObject> SpellSlots;
        private int slotSelected = 0;
        private List<Spell> spells = new();

        public Spell activeSpell {
            get {
                return spells[slotSelected];
            }
        }

        public bool CanAddSpell() {
            return spells.Count < SpellSlots.Count;
        }

        public void AddSpell(Spell spell) {
            if (!CanAddSpell()) {
                Debug.LogError("Already reached the limit of 4 spells");
                return;
            }
            spells.Add(spell);

            var spellSlotObj = SpellSlots[spells.Count - 1];
            spellSlotObj.SetActive(true);
            spellSlotObj.transform.Find("spellicon").GetComponent<Image>().sprite = SpriteManager.Instance.RetrieveSpellSprite(spell.icon);
            // TODO spellSlotObj.transform.Find("manacost").GetComponent<TextMeshProUGUI>().text = "foo";
            // TODO spellSlotObj.transform.Find("damage").GetComponent<TextMeshProUGUI>().text = "bar";
            spellSlotObj.transform.Find("drop").gameObject.SetActive(false);
        }

    }
}