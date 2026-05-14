using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(SpriteRenderer))]
    public class ClassSelectorControl : MonoBehaviour {
        private PlayerClassData playerClass;
        [SerializeField] private Image spriteRenderer;
        [SerializeField] private TextMeshProUGUI textGui;
        
        public void SetPlayerClass(string name, PlayerClassData playerClass) {
            this.playerClass = playerClass;
            spriteRenderer.sprite = GameManager.Instance.enemySpriteManager.Get(playerClass.sprite);
            textGui.text = name;
        }
    }
}
