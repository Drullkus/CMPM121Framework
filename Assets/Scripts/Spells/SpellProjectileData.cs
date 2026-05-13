namespace Spells
{
    public class SpellProjectileData {

        public readonly string trajectory;
        public readonly string speed;
        public readonly string lifetime;
        public readonly int sprite;

		public readonly SpellData onHit;

		public readonly SpellData onCountdown;
		public readonly float countdownPeriod;
		public readonly bool countdownRepeats;

    }

	public class SpellProjectileBlueprint {

		private string trajectory;

	}

}
