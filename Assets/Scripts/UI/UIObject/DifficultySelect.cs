using UnityEngine;

[RequireComponent(typeof(UIObject))]
public class DifficultySelect : MonoBehaviour {
	
	private void Awake() {
		EventBus.Instance.RegisterUIObject(GetComponent<UIObject>());
	}

}
