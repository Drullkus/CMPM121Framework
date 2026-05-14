using System.Collections.Generic;

namespace Spells {

	public class ProjectileBlueprint {

		private Trajectory _trajectory;
		private int _sprite;
		private Dictionary<string, Stat> _fields;

		public ProjectileBlueprint SetTrajectory(Trajectory trajectory) {
			_trajectory = trajectory;

			return this;
		}

		public ProjectileBlueprint SetSprite(int sprite) {
			_sprite = sprite;

			return this;
		}

		public ProjectileBlueprint SetStat(string statName, Stat newStatValue) {
			_fields[statName] = newStatValue;

			return this;
		}

	}

}
