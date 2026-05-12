using UnityEngine;

namespace Spells
{
    public class SpellData
    {
        public string name { get; set; }
        public string description { get; set; }
        public int icon { get; set; }
        public string N { get; set; }
        public SpellDamageData damage { get; set; }
        public string secondaryDamage { get; set; }
        public string manaCost { get; set; }
        public string cooldown { get; set; }
        public SpellProjectileData projectile { get; set; }
        public SpellProjectileData secondaryProjectile { get; set; }
        
        public string delay { get; set; }
        public string damageMultiplier { get; set; }
        public string manaMultiplier { get; set; }
        public string speedMultiplier { get; set; }
        public string manaAdder { get; set; }
        
    }
}