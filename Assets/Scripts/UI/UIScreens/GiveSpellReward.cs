using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiveSpellReward : MonoBehaviour {

    private Spell _spell;
        
    public void RollOption() {
        //_spell = SpellBuilder.GenerateSpell();
        //DisplaySpell();
    }

    private void DisplaySpell() {
        var image = gameObject.GetComponentsInChildren<Image>().FirstOrDefault(c => c.gameObject != gameObject);
        image.sprite = SpriteManager.Instance.RetrieveSpellSprite(_spell.icon);
        
        var textMesh = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = $"{_spell.name}\n{_spell.description}"; // TODO add more info, like modifiers
    }

    public void OnClick() {
        Debug.Log("TODO add _spell into player's spell deck");
    }
}