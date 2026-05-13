namespace Spells
{
	public enum Trajectory {
		STRAIGHT,
		SPIRAL,
		HOMING,
	}

    public class SpellProjectileData
    {
        private readonly Trajectory _trajectory;
        private readonly string _speed;
        private readonly string _lifetime;
        private readonly int _sprite;
    }
}
