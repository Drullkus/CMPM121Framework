using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiveSpellReward : MonoBehaviour {

    private Spell _spell;

    private SpellBuilder _spellBuilder = new SpellBuilder();

    [Header("Hidden upon collecting spell reward")]
    [SerializeField] GameObject deactivatedUponClick;

    [SerializeField] UI.SpellBarManager spellBarManager;

    public void RollOption() {
        deactivatedUponClick.SetActive(true);
        _spell = _spellBuilder.GenerateSpell();

        var image = gameObject.GetComponentsInChildren<Image>().FirstOrDefault(c => c.gameObject != gameObject);
        image.sprite = SpriteManager.Instance.RetrieveSpellSprite(_spell.icon);
        
        var textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = $"{_spell.name}\n{_spell.description}"; // TODO add more info, like modifiers
    }

    public void OnClick() {
        if (!spellBarManager.CanAddSpell()) {
            Debug.LogWarning("All out of slots on the spell bar");
            return;
        }

        deactivatedUponClick.SetActive(false);
        spellBarManager.AddSpell(_spell);
    }
}
