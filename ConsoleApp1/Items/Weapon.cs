namespace ConsoleApp1;

    public class Weapon : AbstractWeapon
    {
        public override WeaponType Type { get; set; }
        public Weapon(string name, WeaponType weaponType, char? symbol = null) : base(name, symbol)
        {
            Type = weaponType;
        }
        
        public override void ApplyModifier(Hero hero)
        {
            return;
        }

        public override void RemoveModifier(Hero hero)
        {
            return;
        }
    }