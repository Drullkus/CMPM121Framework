using Newtonsoft.Json;

namespace Spells {

    public class SpellData {

		[JsonProperty("name")]
		private string _name;
		[JsonProperty("description")]
		private string _description;
		[JsonProperty("icon")]
		private string _icon;

		[JsonProperty("damage")]
		private SpellDamageData _primaryDamage;
		[JsonProperty("secondary_damage")]
		private SpellDamageData _secondaryDamage;

		[JsonProperty("projectile")]
		private ProjectileData _primaryProjectile;
		[JsonProperty("secondary_projectile")]
		private ProjectileData _secondaryProjectile;

		[JsonProperty("cooldown")]
		private string _cooldown;
		[JsonProperty("mana_cost")]
		private string _manaCost;

		[JsonProperty("delay")]
		private string _delay;
		[JsonProperty("N")]
		private string _n;
		[JsonProperty("spray")]
		private string _spray;

    }

}
