namespace Spells
{
	public enum Trajectory {
		STRAIGHT,
		SPIRAL,
		HOMING,
	}

    public class ProjectileData
    {
        private readonly Trajectory _trajectory;
        private readonly string _speed;
        private readonly string _lifetime;
        private readonly int _sprite;

		public ProjectileData(
			Trajectory trajectory,
			string speed,
			string lifetime,
			int sprite
		) {
			_trajectory = trajectory;
			_speed = speed;
			_lifetime = lifetime;
			_sprite = sprite;
		}
    }
}
