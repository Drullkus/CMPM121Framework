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
			int sprite,
			string speed,
			string lifetime
		) {
			_trajectory = trajectory;
			_sprite = sprite;

			_speed = speed;
			_lifetime = lifetime;
		}

		ProjectileBlueprint MakeBlueprint() {
			ProjectileBlueprint blueprint = new();

			blueprint.SetTrajectory(_trajectory);
			blueprint.SetSprite(_sprite);

			blueprint.SetStat("speed", new(_speed));
			blueprint.SetStat("lifetime", new(_lifetime));

			return blueprint;
		}
    }
}
