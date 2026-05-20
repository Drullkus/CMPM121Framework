using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class SpellUI : MonoBehaviour {

    public GameObject icon;
    public RectTransform cooldown;
    public TextMeshProUGUI manacost;
    public TextMeshProUGUI damage;
    public GameObject highlight;
    public Spell spell;
    float lastTextUpdate;
    const float UPDATE_DELAY = 1;
    public GameObject dropbutton;

    void Start() {
        lastTextUpdate = 0;
    }

    public void SetSpell(Spell spell) {
        this.spell = spell;
		icon.GetComponent<Image>().sprite = SpriteManager.Instance.RetrieveSpellSprite(spell.GetIcon());
    }

    void Update() {
        if (spell == null) { return; }
        if (Time.time > lastTextUpdate + UPDATE_DELAY) {
            manacost.text = spell.GetManaCost().ToString();
            damage.text = spell.GetDamage().ToString();
            lastTextUpdate = Time.time;
        }
        
        float since_last = Time.time - spell.last_cast;
        float perc;
        if (since_last > spell.GetCooldown()) {
            perc = 0;
        } else {
            perc = 1-since_last / spell.GetCooldown();
        }
        cooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48 * perc);
    }

}
