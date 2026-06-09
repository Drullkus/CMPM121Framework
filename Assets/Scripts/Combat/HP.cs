using System;

public class HP {
	
	private int _maxValue;
	private int _value;

	private HealthBar _healthBar;

	public event Action OnExpended;

	public HP(int value, HealthBar healthBar) {
		_maxValue = value;
		_value = value;

		_healthBar = healthBar;
		HPChanged();
	}

	public virtual void TakeDamage(Damage damage) {
		_value -= damage.amount;

		HPChanged();

		if(_value <= 0) {
			_value = 0;
			OnExpended?.Invoke();
		}
	}

	public void Recover(int amount) {
		_value = Math.Clamp(_value + amount, 0, _maxValue);

		HPChanged();
	}

	public void RecoverFull() {
		_value = _maxValue;

		HPChanged();
	}

	private void HPChanged() {
		if(_healthBar == null) { return; }

		_healthBar.SetHealth((float)_value / (float)_maxValue);
	}

}
