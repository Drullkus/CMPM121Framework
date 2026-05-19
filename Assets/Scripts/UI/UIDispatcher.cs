public class UIDispatcher {
	
	public enum UIState {
		REWARD,
		WAVE,
	}

	private UIState _state;

	private static UIDispatcher _instance;
	public static UIDispatcher Instance {
		get {
			_instance ??= new();
			return _instance;
		}
	}

	private UIDispatcher() { }

	private void ChangeState(UIState newState) { }

}

