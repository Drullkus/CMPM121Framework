using Newtonsoft.Json;

namespace Spells {

    public class SpellData {

		[JsonProperty("name")]
		private readonly string _name;
		[JsonProperty("description")]
		private readonly string _description;
		[JsonProperty("icon")]
		private readonly string _icon;

		[JsonProperty("damage")]
		private readonly SpellDamageData _primaryDamage;
		[JsonProperty("secondary_damage")]
		private readonly SpellDamageData _secondaryDamage;

		[JsonProperty("projectile")]
		private ProjectileData _primaryProjectile;
		[JsonProperty("secondary_projectile")]
		private ProjectileData _secondaryProjectile;

		[JsonProperty("cooldown")]
		private readonly string _cooldown;
		[JsonProperty("mana_cost")]
		private readonly string _manaCost;

		[JsonProperty("delay")]
		private readonly string _delay;
		[JsonProperty("N")]
		private readonly string _n;
		[JsonProperty("spray")]
		private readonly string _spray;

    }

}
