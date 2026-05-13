using System.Collections.Generic;

namespace Spells
{
    public class SpellData
    {

        public readonly string name;
        public readonly string description;
        public readonly int icon;
        public string manaCost;
        public string cooldown;
        public SpellDamageData damageData;
		public SpellDamageData secondaryDamageData;
        public SpellProjectileData projectileData;
		public SpellProjectileData secondaryProjectileData;
        public string delay;

		public string n;
        
		private List<SpellBlueprintModifier> _modifiers;

		public SpellBlueprint MakeBlueprint() {
			SpellBlueprint blueprint = new(this);
			
			foreach(SpellBlueprintModifier modifier in _modifiers) {
				blueprint.Modify(modifier);
			}

			return blueprint;
		}

		public SpellData(string name, string description, int icon) {
			this.name = name;
			this.description = description;
			this.icon = icon;
		}

		public SpellData SetManaCost(string manaCost) {
			this.manaCost = manaCost;
			return this;
		}

		public SpellData SetCooldown(string cooldown) {
			this.cooldown = cooldown;
			return this;
		}

		public SpellData SetDamageData(SpellDamageData damageData) {
			this.damageData = damageData;
			return this;
		}

		public SpellData SetProjectileData(ProjectileData projectileData) {
			this.projectileData = projectileData;
			return this;
		}

		public SpellData SetDelay(string delay) {
			this.delay = delay;
			return this;
		}

		public SpellData SetN(string n) {
			this.N = n;
			return this;
		}

		public SpellData SetSecondaryDamage(SpellDamageData damageData) {
			secondaryDamageData = damageData;
			return this;
		}

		public SpellData SetSecondaryProjectileData(ProjectileData projectileData) {
			secondaryProjectileData = projectileData;
			return this;
		}

		// TODO - add path
		public SpellData SetDamageMultiplier(string multiplier) {
			SpellBlueprintRPNStatModifier modifier = new(multiplier, "*", null);

			_modifiers.Add(modifier);

			return this;
		}

		// TODO - add path
		public SpellData SetManaAdder(string amount) {
			SpellBlueprintRPNStatModifier modifier = new(amount, "+", null);

			_modifiers.Add(modifier);

			return this;
		}

		// TODO - add path
		public SpellData SetManaMultiplier(string multiplier) {
			SpellBlueprintRPNStatModifier modifier = new(multiplier, "*", null);

			_modifiers.Add(modifier);

			return this;
		}

		public SpellData SetSpeedMultiplier(string multiplier) {
			SpellBlueprintRPNStatModifier modifier = new(multiplier, "*", null);

			_modifiers.Add(modifier);

			return this;
		}

    }

	public class SpellBlueprint {
		
		public readonly string name;
		public readonly string description;
		public readonly int iconIndex;

		private Dictionary<string, string> _mutableValues = new();

		private SpellDamageData damageData;
		private SpellProjectileData projectileData;

		public SpellBlueprint(SpellData spellData) {
			name = spellData.name;
			description = spellData.description;
			iconIndex = spellData.icon;

			_mutableValues.Add("manaCost", spellData.manaCost);
			_mutableValues.Add("cooldown", spellData.cooldown);
			_mutableValues.Add("delay", spellData.delay);
		}

		public void Modify(SpellBlueprintModifier modifier) {
			modifier.Transform(this);
		}

		public void Cast() {}
		
	}

	public class SpellStatPath {

		

		public SpellBlueprint ResolveSpellReference(SpellBlueprint root) {
			
		}

		public string ResolveRPNString(SpellBlueprint root) {
			
		}

	}

	public abstract class SpellBlueprintModifier {

		public virtual void Transform(SpellBlueprint blueprint);

	}

	public class SpellBlueprintRPNStatModifier : SpellBlueprintModifier {
		
		private string _prefix;
		private string _suffix;

		private SpellStatPath _path;

		public SpellBlueprintRPNStatModifier(string prefix, string suffix, SpellStatPath path) {
			_prefix = prefix;
			_suffix = suffix;
			_path = path;
		}

		public void Transform(SpellBlueprint blueprint) {
			_path.ResolveRPNString(blueprint) = $"{_prefix} {_path.ResolveRPNString(blueprint)} {_suffix}";
		}

	}

	public class SpellBlueprintSpellReferenceStatModifier : SpellBlueprintModifier{
		
		private readonly SpellBlueprint _replacementSpellBlueprint;

		private readonly SpellStatPath _pathToReplace;

		private void Transform(SpellBlueprint blueprint) {
			_pathToReplace.ResolveSpellReference(blueprint) = _replacementSpellBlueprint;
		}

	}

}
