using System;

public class HP {
	
	private int _maxValue;
	private int _value;

	public event Action OnExpended;

	public virtual void TakeDamage(Damage damage) {
		_value -= damage.amount;
		if(_value <= 0) {
			_value = 0;
			OnExpended?.Invoke();
		}
	}

	public void Recover(int amount) {
		_value += amount;
		Math.Clamp(_value, 0, _maxValue);
	}

	public void RecoverFull() {
		_value = _maxValue;
	}

	public HP(int value) {
		_maxValue = value;
		_value = value;
	}

}
