using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Button))]
    public class ClassSelectorControl : MonoBehaviour {

        [SerializeField] private Image spriteRenderer;
        [SerializeField] private TextMeshProUGUI textGui;
        [SerializeField] private Button button;
        
        public void SetPlayerClass(string name, PlayerClassData playerClass, UnityAction onClick) {
            spriteRenderer.sprite = SpriteManager.Instance.RetrievePlayerSprite(playerClass.sprite);
            textGui.text = name;
            button.onClick.AddListener(onClick);
        }

    }
}
