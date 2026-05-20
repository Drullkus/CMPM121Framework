using System;

public class HP {
	
	private int _value;

	public event Action OnHPExpended;

	public virtual void TakeDamage(Damage damage) {
		_value -= damage.amount;
		if(_value <= 0) {
			_value = 0;
			OnHPExpended?.Invoke();
		}
	}

	public HP(int value) {
		_value = value;
	}

}
