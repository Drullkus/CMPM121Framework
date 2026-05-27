using System;

public class UIScreen {

	public UIState state = UIState.WAVE;

	private readonly Action _show;
	private readonly Action _hide;

	public UIScreen(UIState state, Action show, Action hide) {
		this.state = state;
		
		_show = show;
		_hide = hide;
	}

	public void Show() { _show.Invoke(); }
	public void Hide() { _hide.Invoke(); }

}
