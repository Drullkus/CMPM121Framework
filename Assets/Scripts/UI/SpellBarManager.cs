using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class SpellBarManager : MonoBehaviour {

        [SerializeField] private GameManager _gameManager;
        [SerializeField] private List<GameObject> SpellSlots;
        private int slotSelected = 0;
        private List<Spell> spells = new();

		private void Awake() {
			SceneManager.sceneLoaded += (_, _) => {
				spells.Clear();
			};
		}

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
            spellSlotObj.transform.Find("drop").gameObject.SetActive(false);

            if (spells.Count - 1 != slotSelected) {
                spellSlotObj.transform.Find("highlight").gameObject.SetActive(false);
            }

            var spellTraits = spell.GetTraits(new List<string> {"damage.amount", "manaCost"});
            
            Debug.Log($"damage.amount: {spellTraits[0].Item2.traitValue}, manaCost: {spellTraits[1].Item2.traitValue}");

            var waveVal = _gameManager.getWave();
            var spellPowerVal = 100;

            var damageVal = Math.Ceiling(RPNEvaluator.RPNEvaluator.Evaluatef(spellTraits[0].Item2.traitValue, new Dictionary<string, int>() { ["power"] = spellPowerVal, ["wave"] = waveVal }));
            var manaVal = Math.Ceiling(RPNEvaluator.RPNEvaluator.Evaluatef(spellTraits[1].Item2.traitValue, new Dictionary<string, int>() { ["power"] = spellPowerVal, ["wave"] = waveVal }));

            Debug.Log($"damageVal: {damageVal}, manaVal: {manaVal}");
            
            spellSlotObj.transform.Find("damage").GetComponent<TextMeshProUGUI>().text = $"{damageVal}";
            spellSlotObj.transform.Find("manacost").GetComponent<TextMeshProUGUI>().text = $"{manaVal}";
        }

        public void ToggleSpellSlot() {
            if (spells.Count <= 0) {
                return;
            }

            SetSlot(slotSelected + 1);
        }

        public void ChooseSpell(int numKeyPressed) {
            if (numKeyPressed > spells.Count) {
                return;
            }

            SetSlot(numKeyPressed - 1);
        }

        private void SetSlot(int newSelected) {
            int oldSelected = slotSelected;
            slotSelected = newSelected % spells.Count;
            
            SpellSlots[oldSelected].transform.Find("highlight").gameObject.SetActive(false);
            SpellSlots[slotSelected].transform.Find("highlight").gameObject.SetActive(true);
        }

    }
}
