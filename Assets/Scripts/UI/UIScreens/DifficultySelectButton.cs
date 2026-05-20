using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DifficultySelectButton : MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI _label;

	public void Initialize(Vector2 position, Level level) {
		_label.text = level.Name;

		Button button = gameObject.GetComponent<Button>();

		transform.localPosition = (Vector3)position;

		button.onClick.AddListener(() => {
			EventBus.Instance.ChooseDifficulty(level);
			EventBus.Instance.CloseUIScreen();
		});
	}

}
