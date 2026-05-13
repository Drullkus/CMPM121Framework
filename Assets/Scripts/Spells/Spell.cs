using System.Collections.Generic;

namespace Spells
{
    public class SpellData
    {

        public readonly string name;
        public readonly string description;
        public readonly int icon;
        public readonly string manaCost;
        public readonly string cooldown;
        public readonly SpellDamageData damageData;
        public readonly SpellProjectileData projectileData;
        public readonly string delay; 
        
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

			return this;
		}

		public SpellData SetSecondaryDamage() {

			return this;
		}

		public SpellData SetSecondaryProjectileData() {

			return this;
		}
		public SpellData SetDamageMultiplier() {

			return this;
		}

		public SpellData SetManaAdder() {

			return this;
		}

		public SpellData SetManaMultiplier() {

			return this;
		}

		public SpellData SetSpeedMultiplier() {

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

		public void Transform(SpellBlueprint blueprint) {
			// rpnStat = $"{_prefix} {rpnStat} {_suffix}";
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
